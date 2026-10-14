// 격리 진단 전용입니다. 원본 PNG/mesh/Transform을 이동하지 않고 겹치는 Actor 영역의 alpha만 낮춥니다.
Shader "Limitless/ArbelLocalizedRoof"
{
 Properties {
  [PerRendererData] _MainTex ("Sprite Texture",2D)="white" {}
  _Color("Tint",Color)=(1,1,1,1)
  _ActorTex0("Actor 0",2D)="black" {}
  _ActorUV0("Actor UV",Vector)=(0,0,1,1)
  _ActorPivotPPU0("Pivot PPU enabled",Vector)=(0,0,128,0)
  _ActorSizeFlip0("Size flip",Vector)=(128,128,1,1)
  _ActorTex1("Actor 1",2D)="black" {}
  _ActorUV1("Actor UV",Vector)=(0,0,1,1)
  _ActorPivotPPU1("Pivot PPU enabled",Vector)=(0,0,128,0)
  _ActorSizeFlip1("Size flip",Vector)=(128,128,1,1)
  _ActorTex2("Actor 2",2D)="black" {}
  _ActorUV2("Actor UV",Vector)=(0,0,1,1)
  _ActorPivotPPU2("Pivot PPU enabled",Vector)=(0,0,128,0)
  _ActorSizeFlip2("Size flip",Vector)=(128,128,1,1)
  _ActorTex3("Actor 3",2D)="black" {}
  _ActorUV3("Actor UV",Vector)=(0,0,1,1)
  _ActorPivotPPU3("Pivot PPU enabled",Vector)=(0,0,128,0)
  _ActorSizeFlip3("Size flip",Vector)=(128,128,1,1)
  _LowAlpha("Local alpha",Float)=0.18
  _ProtectedY("Keep eave opaque below Y",Float)=4.09375
  _Badge("Keep badge center/radius",Vector)=(-4.5,4.125,0.24,0)
  _Seam("Join x min/max / y min/max",Vector)=(-5.5,-3.5,3.9375,4.09375)
  _JoinEnabled("Join transparent seam",Float)=0
 }
 SubShader {
  Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "CanUseSpriteAtlas"="True" }
  Cull Off Lighting Off ZWrite Off Blend One OneMinusSrcAlpha
  Pass {
   CGPROGRAM
   #pragma vertex vert
   #pragma fragment frag
   #pragma multi_compile_instancing
   #pragma multi_compile _ PIXELSNAP_ON
   #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
   #include "UnitySprites.cginc"
   float4 _MainTex_TexelSize,_Badge,_Seam;
   sampler2D _ActorTex0; float4 _ActorUV0,_ActorPivotPPU0,_ActorSizeFlip0; float4x4 _ActorWorldToLocal0;
   sampler2D _ActorTex1; float4 _ActorUV1,_ActorPivotPPU1,_ActorSizeFlip1; float4x4 _ActorWorldToLocal1;
   sampler2D _ActorTex2; float4 _ActorUV2,_ActorPivotPPU2,_ActorSizeFlip2; float4x4 _ActorWorldToLocal2;
   sampler2D _ActorTex3; float4 _ActorUV3,_ActorPivotPPU3,_ActorSizeFlip3; float4x4 _ActorWorldToLocal3;
   float _LowAlpha,_ProtectedY,_JoinEnabled;
   struct output {float4 vertex:SV_POSITION;float2 uv:TEXCOORD0;float2 world:TEXCOORD1;fixed4 color:COLOR;};
   output vert(appdata_t v){
    output o;UNITY_SETUP_INSTANCE_ID(v);float4 pos=UnityFlipSprite(v.vertex,_Flip);o.vertex=UnityObjectToClipPos(pos);o.world=mul(unity_ObjectToWorld,pos).xy;o.uv=v.texcoord;o.color=v.color*_Color*_RendererColor;
    #ifdef PIXELSNAP_ON
    o.vertex=UnityPixelSnap(o.vertex);
    #endif
    return o;
   }
   float actorAlpha0(float2 world){
    float2 local=mul(_ActorWorldToLocal0,float4(world,0,1)).xy*_ActorSizeFlip0.zw;
    float2 frame=(local*_ActorPivotPPU0.z+_ActorPivotPPU0.xy)/_ActorSizeFlip0.xy;
    float inside=step(0,frame.x)*step(frame.x,1)*step(0,frame.y)*step(frame.y,1);
    return tex2D(_ActorTex0,_ActorUV0.xy+frame*_ActorUV0.zw).a*inside*_ActorPivotPPU0.w;
   }
   float actorAlpha1(float2 world){
    float2 local=mul(_ActorWorldToLocal1,float4(world,0,1)).xy*_ActorSizeFlip1.zw;
    float2 frame=(local*_ActorPivotPPU1.z+_ActorPivotPPU1.xy)/_ActorSizeFlip1.xy;
    float inside=step(0,frame.x)*step(frame.x,1)*step(0,frame.y)*step(frame.y,1);
    return tex2D(_ActorTex1,_ActorUV1.xy+frame*_ActorUV1.zw).a*inside*_ActorPivotPPU1.w;
   }
   float actorAlpha2(float2 world){
    float2 local=mul(_ActorWorldToLocal2,float4(world,0,1)).xy*_ActorSizeFlip2.zw;
    float2 frame=(local*_ActorPivotPPU2.z+_ActorPivotPPU2.xy)/_ActorSizeFlip2.xy;
    float inside=step(0,frame.x)*step(frame.x,1)*step(0,frame.y)*step(frame.y,1);
    return tex2D(_ActorTex2,_ActorUV2.xy+frame*_ActorUV2.zw).a*inside*_ActorPivotPPU2.w;
   }
   float actorAlpha3(float2 world){
    float2 local=mul(_ActorWorldToLocal3,float4(world,0,1)).xy*_ActorSizeFlip3.zw;
    float2 frame=(local*_ActorPivotPPU3.z+_ActorPivotPPU3.xy)/_ActorSizeFlip3.xy;
    float inside=step(0,frame.x)*step(frame.x,1)*step(0,frame.y)*step(frame.y,1);
    return tex2D(_ActorTex3,_ActorUV3.xy+frame*_ActorUV3.zw).a*inside*_ActorPivotPPU3.w;
   }
   fixed4 frag(output i):SV_Target{
    fixed4 c=SampleSpriteTexture(i.uv);
    // 실제 알파 여백만 채웁니다. 바로 위 첫 불투명 처마 픽셀을 재사용하며 PNG나 지붕 위치를 바꾸지 않습니다.
    if(_JoinEnabled>.5 && i.world.x>=_Seam.x && i.world.x<=_Seam.y && i.world.y>=_Seam.z && i.world.y<=_Seam.w && c.a<.5){
     [unroll]for(int n=1;n<=24;n++){fixed4 above=SampleSpriteTexture(i.uv+float2(0,n*_MainTex_TexelSize.y));if(above.a>=.9){c=above;break;}}
    }
    float mask=max(max(actorAlpha0(i.world),actorAlpha1(i.world)),max(actorAlpha2(i.world),actorAlpha3(i.world)));
    // 처마/표지/실루엣 외곽은 alpha1을 유지해 건축 연결과 앞쪽 가림 단서를 보존합니다.
    float contour=min(min(SampleSpriteTexture(i.uv+float2(4*_MainTex_TexelSize.x,0)).a,SampleSpriteTexture(i.uv-float2(4*_MainTex_TexelSize.x,0)).a),min(SampleSpriteTexture(i.uv+float2(0,4*_MainTex_TexelSize.y)).a,SampleSpriteTexture(i.uv-float2(0,4*_MainTex_TexelSize.y)).a));
    if(i.world.y<=_ProtectedY || distance(i.world,_Badge.xy)<=_Badge.z || contour<.5)mask=0;
    c*=i.color;c.a*=lerp(1,_LowAlpha,mask);c.rgb*=c.a;return c;
   }
   ENDCG
  }
 }
}
