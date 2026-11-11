using System;
using System.Collections.Generic;
using System.Linq;
using ProjectLimitless.World;
using UnityEngine;


/// <summary>첫눈의 숲 승인 눈길 기반을 한 번 생성합니다. Scene에 명시적으로 추가해야 동작하며 Collider·나무·HUD는 변경하지 않습니다.</summary>
public sealed class F2SnowPathBuilder : MonoBehaviour
{

    // 기존 PNG/GUID는 바꾸지 않고 Scene에서 승인 Sprite를 연결하도록 분리합니다.
    [SerializeField] Sprite[] groundSprites = new Sprite[9]; // Earth3 → ThinFrost3 → Gravel3
    [SerializeField] Sprite firstSnow;
    [SerializeField] Sprite wornTrail;
    bool built;
    SpriteRenderer[] hiddenRenderers = Array.Empty<SpriteRenderer>();
    bool[] previousEnabled = Array.Empty<bool>();
    readonly List<Mesh> meshes = new List<Mesh>();
    readonly List<Material> materials = new List<Material>();
    readonly Dictionary<Texture, List<Mesh>> trailParts = new Dictionary<Texture, List<Mesh>>();
    public int MovedSnowCount { get; private set; }
    // 격리 검증 또는 향후 Scene 조립 도구가 승인 Sprite 참조를 지정하는 진입점입니다.
    public void Configure(Sprite[] ground, Sprite snow, Sprite worn)
    {
        if (built) throw new InvalidOperationException("생성 후 입력을 교체할 수 없습니다.");
        groundSprites=ground; firstSnow=snow; wornTrail=worn;
    }
    void Start() { if (!built) Build(); }
    struct Placement
    {
        public string Name; public Vector2 Before, After; public float Angle;
        public Placement(string name,float bx,float by,float ax,float ay,float angle)
        { Name=name;Before=new Vector2(bx,by);After=new Vector2(ax,ay);Angle=angle; }
    }
    // 17b045e0의 B-placement-diff.csv를 동결한 값입니다. 동명 오브젝트는 이전 좌표로 식별합니다.
    static readonly Placement[] ApprovedPlacement = {
        new Placement("FirstSnow_5_4", -4.83f, -3f, -4.95f, -3.23f, -9f),
        new Placement("FirstSnow_5_6", -4.83f, 0.87f, -5.04f, 1.03f, 6f),
        new Placement("FirstSnow_5_7", -4.83f, 3f, -4.95f, 2.77f, -9f),
        new Placement("FirstSnow_6_7", -3.17f, 3f, -3.29f, 2.77f, -9f),
        new Placement("Snow_PathEdge_REUSE", -3.3f, -4.200001f, -3.6f, -4.280001f, 11f),
        new Placement("Snow_PathEdge_REUSE", -3.3f, -2.500001f, -3.51f, -2.340001f, 6f),
        new Placement("Snow_PathEdge_REUSE", 6.3f, -2.500001f, 6.6f, -2.580001f, 11f),
        new Placement("Snow_PathEdge_REUSE", -3.3f, -0.8000007f, -3.42f, -1.030001f, -9f),
        new Placement("Snow_PathEdge_REUSE", 6.3f, -0.8000007f, 6.69f, -0.5300007f, -4f),
        new Placement("Snow_PathEdge_REUSE", -3.3f, 0.8999994f, -3.69f, 1.169999f, -4f),
        new Placement("Snow_PathEdge_REUSE", 6.3f, 0.8999994f, 6.42f, 0.6699994f, -9f),
        new Placement("Snow_PathEdge_REUSE", -3.3f, 2.599999f, -3.6f, 2.52f, 11f),
        new Placement("Snow_PathEdge_REUSE", 6.3f, 2.599999f, 6.51f, 2.76f, 6f),
        new Placement("Snow_PathEdge_REUSE", -3.3f, 4.299999f, -3.51f, 4.459999f, 6f),
    };
    void ApplyApprovedPlacement()
    {
        var all=GetComponentsInChildren<SpriteRenderer>();
        foreach(var p in ApprovedPlacement)
        {
            var matches=all.Where(x=>x.name==p.Name && Vector2.Distance(x.transform.position,p.Before)<.00002f).ToArray();
            if(matches.Length!=1) throw new InvalidOperationException("B 배치 대상 불일치: "+p.Name);
            var t=matches[0].transform;
            // CSV는 소수점을 반올림해 저장합니다. 원래 B 검증기의 float 연산을 그대로 재현해
            // 미세한 좌표 양자화로 텍셀 경계가 바뀌는 것을 방지하고, 기록 좌표와의 오차도 제한합니다.
            int index=p.Angle==-9f?0:p.Angle==6f?1:p.Angle==11f?2:3;
            float dx=(p.Before.x<0?-1:1)*(.12f+.09f*index);
            float dy=new[]{-.23f,.16f,-.08f,.27f}[index];
            var restored=t.position+new Vector3(dx,dy,0);
            if(Vector2.Distance(restored,p.After)>.000002f) throw new InvalidOperationException("승인 B 좌표 기록과 불일치: "+p.Name);
            t.position=restored;
            t.rotation=Quaternion.Euler(0,0,p.Angle); MovedSnowCount++;
        }
    }
    // 같은 텍스처/Sorting의 길 조각만 묶습니다. 정점·UV·색상·삼각형은 그대로 복사합니다.
    // Ground와 눈 Sprite는 합치지 않아 투명 정렬과 향후 개별 배치 편집을 유지합니다.
    void CombineTrailMeshes()
    {
        foreach(var group in trailParts)
        {
            var combined=new Mesh { name="F2_ApprovedTrail_Combined" };
            combined.CombineMeshes(group.Value.Select(x=>new CombineInstance { mesh=x, transform=Matrix4x4.identity }).ToArray(),true,true);
            var holder=new GameObject("Trail_4p5WU_Combined",typeof(MeshFilter),typeof(MeshRenderer));
            holder.transform.SetParent(transform,false);holder.GetComponent<MeshFilter>().sharedMesh=combined;
            var material=new Material(Shader.Find("Sprites/Default")) { mainTexture=group.Key };
            var renderer=holder.GetComponent<MeshRenderer>();renderer.sharedMaterial=material;renderer.sortingOrder=-26;
            meshes.Add(combined);materials.Add(material);
        }
        trailParts.Clear();
        // 결합 후 사용하지 않는 원본 Mesh/Material은 생성 프레임이 끝나면 해제합니다.
        var usedMeshes=new HashSet<Mesh>(GetComponentsInChildren<MeshFilter>().Select(x=>x.sharedMesh));
        var usedMaterials=new HashSet<Material>(GetComponentsInChildren<MeshRenderer>().Select(x=>x.sharedMaterial));
        foreach(var mesh in meshes.Where(x=>!usedMeshes.Contains(x)).ToArray()){Destroy(mesh);meshes.Remove(mesh);}
        foreach(var material in materials.Where(x=>!usedMaterials.Contains(x)).ToArray()){Destroy(material);materials.Remove(material);}
    }
    void OnDestroy()
    {
        // Scene 재진입 시 생성 Mesh/Material이 누적되지 않도록 이 컴포넌트 소유 자원만 정리합니다.
        for(int i=0;i<hiddenRenderers.Length;i++) if(hiddenRenderers[i]!=null) hiddenRenderers[i].enabled=previousEnabled[i];
        foreach(var mesh in meshes) if(mesh!=null) Destroy(mesh);
        foreach(var material in materials) if(material!=null) Destroy(material);
    }

    // Scene 진입 시 한 번만 구성합니다. 반복 호출은 기존 결과를 반환하고 참조 누락은 명시적으로 실패합니다.
    public GameObject Build()
    {
        if (built) return gameObject;
        if (gameObject.scene.name != Main24F2Traversal.SceneId)
            throw new InvalidOperationException("첫눈의 숲 Scene에서만 생성할 수 있습니다.");
        // 입력이 없으면 기존 지면을 숨기지 않습니다. 본편 연결은 별도 Scene 컴포넌트 승인 후에만 합니다.
        if (groundSprites == null || groundSprites.Length != 9 || groundSprites.Any(x=>x==null) || firstSnow == null || wornTrail == null)
            throw new InvalidOperationException("승인 Ground9/FirstSnow/WornTrail 참조를 모두 지정해야 합니다.");
        if(transform.position!=Vector3.zero || transform.rotation!=Quaternion.identity || transform.lossyScale!=Vector3.one || transform.childCount!=0)
            throw new InvalidOperationException("빈 Root의 위치0·회전0·배율1이 필요합니다.");
        if(FindObjectsByType<F2SnowPathBuilder>(FindObjectsSortMode.None).Any(x=>x!=this && x.gameObject.scene==gameObject.scene))
            throw new InvalidOperationException("동일 Scene에 눈길 Builder를 중복 설치할 수 없습니다.");
        var root=gameObject;
        // 이 Scene의 눈바닥1·흙길3만 숨깁니다. 나무·그루터기·올빼미·Collider·HUD는 대상이 아닙니다.
        var placeholders=UnityEngine.Object.FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None).Where(x=>x.gameObject.scene==gameObject.scene && (x.name=="PLACEHOLDER_SnowGround_ValidationOnly" || x.name.StartsWith("PLACEHOLDER_FirmTrail_"))).ToArray();
        if(placeholders.Length!=4) throw new InvalidOperationException("기존 눈바닥1·흙길3 구성이 일치하지 않습니다.");
        hiddenRenderers=placeholders;
        previousEnabled=placeholders.Select(x=>x.enabled).ToArray();
        var earth=groundSprites.Take(3).ToArray();
        var frost=groundSprites.Skip(3).Take(3).ToArray();
        var gravel=groundSprites.Skip(6).Take(3).ToArray();
        var snow=firstSnow;
        var worn=wornTrail;
        // 256px/PPU128 원본 배율을 유지합니다. 전체 지면을 한 장으로 늘리지 않습니다.
        for(int y=0;y<12;y++)for(int x=0;x<16;x++){
            var at=new Vector2(-15+x*2,-11+y*2);int index=(x*5+y*7)%3;
            Sprite tile=Distance(at)<2.5f?earth[index]:((x+y)%4==0?gravel[index]:frost[index]);
            var ground=Visual(root.transform,"Ground_"+x+"_"+y,tile,at,-30);ground.transform.position=(Vector3)(at-(Vector2)tile.bounds.center);
            if(Distance(at)>3.35f && (x*3+y)%4!=0)Visual(root.transform,"FirstSnow_"+x+"_"+y,snow,at+new Vector2(.17f*((x%3)-1),.13f*((y%3)-1)),-27);
        }
        // 바닥 타일 원본 Pivot은 하단이므로 위에서 bounds 중심을 보정합니다. 눈은 통로 바깥에만 두어 길 폭을 보존합니다.
        for(float y=-11;y<=8;y+=1.7f)foreach(float x in new[]{-3.3f,3.3f,-.3f,6.3f}){
            var at=new Vector2(x,y);if(Distance(at)>3.1f)Visual(root.transform,"Snow_PathEdge_REUSE",snow,at,-27);
        }
        // 통로 중심선 양쪽2.25WU는 유지하고 바깥0.22WU만 투명하게 풀어 직각 끝선을 완화합니다.
        // 기존 PNG UV를 반복하는 Mesh이며 새 래스터 이미지를 만들지 않습니다.
        for(int y=0;y<12;y++)for(int x=0;x<16;x++)PathPatch(root.transform,new Vector2(-16+x*2,-12+y*2),earth[(x+y)%3].texture);
        for(float y=-11;y<=-1;y+=1.7f)Visual(root.transform,"Worn_South",worn,new Vector2(0,y),-24);
        for(float y=1;y<=8;y+=1.7f)Visual(root.transform,"Worn_North",worn,new Vector2(3,y),-24);
        var turn=Visual(root.transform,"Worn_Bend",worn,new Vector2(1.5f,0),-24);turn.transform.rotation=Quaternion.Euler(0,0,90);
        ApplyApprovedPlacement();
        CombineTrailMeshes();
        foreach(var view in hiddenRenderers)view.enabled=false;
        built=true;
        return root;
    }
    static SpriteRenderer Visual(Transform parent,string name,Sprite sprite,Vector2 at,int order)
    {
        if(sprite==null)throw new Exception("재사용 Sprite 누락: "+name);
        var g=new GameObject(name,typeof(SpriteRenderer));g.transform.SetParent(parent,false);g.transform.position=at;
        var r=g.GetComponent<SpriteRenderer>();r.sprite=sprite;r.sortingOrder=order;r.spriteSortPoint=SpriteSortPoint.Pivot;return r;
    }
    static float Segment(Vector2 p,Vector2 a,Vector2 b){var ab=b-a;return Vector2.Distance(p,a+ab*Mathf.Clamp01(Vector2.Dot(p-a,ab)/ab.sqrMagnitude));}
    public static float Distance(Vector2 p)=>Mathf.Min(Segment(p,new Vector2(0,-12),Vector2.zero),Mathf.Min(Segment(p,Vector2.zero,new Vector2(3,0)),Segment(p,new Vector2(3,0),new Vector2(3,8))));
    void PathPatch(Transform parent,Vector2 origin,Texture texture)
    {
        if(Distance(origin+Vector2.one)>4)return;
        const int n=16;var vertices=new Vector3[(n+1)*(n+1)];var uv=new Vector2[vertices.Length];var colors=new Color[vertices.Length];var triangles=new List<int>();
        for(int y=0;y<=n;y++)for(int x=0;x<=n;x++){int i=y*(n+1)+x;var p=origin+new Vector2(2f*x/n,2f*y/n);vertices[i]=p;uv[i]=new Vector2((float)x/n,(float)y/n);float d=Distance(p);float edge=2.25f+.03f*(1+Mathf.Sin(p.x*4+p.y*3));float a=1-Mathf.SmoothStep(0,1,Mathf.InverseLerp(edge,edge+.22f,d));colors[i]=new Color(1,1,1,a);if(x<n&&y<n){triangles.AddRange(new[]{i,i+n+1,i+1,i+1,i+n+1,i+n+2});}}
        var mesh=new Mesh{name="ExistingPNG_Path_NoRasterGeneration"};mesh.vertices=vertices;mesh.uv=uv;mesh.colors=colors;mesh.triangles=triangles.ToArray();mesh.RecalculateBounds();
        // 중간 조각은 Mesh 데이터로만 보관합니다. 조각별 GameObject/Material 53개를 만들지 않습니다.
        if(!trailParts.TryGetValue(texture,out var parts)) { parts=new List<Mesh>();trailParts.Add(texture,parts); }
        parts.Add(mesh);meshes.Add(mesh);
    }
}
