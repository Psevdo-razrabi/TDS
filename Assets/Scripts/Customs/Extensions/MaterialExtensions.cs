using UnityEngine;

namespace Customs
{
    public static class MaterialExtensions
    {
        private const int MATERIAL_OPAQUE = 0;
        private const int MATERIAL_TRANSPARENT = 1;
        public static void ToOpaqueMode(this Material material)
        {
            material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.One);
            material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.Zero);
            material.SetInt("_ZWrite", 1);
            material.DisableKeyword("_ALPHATEST_ON");
            material.DisableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = -1;
        }
   
        public static void ToFadeMode(this Material material)
        {
            material.SetFloat("_Mode", 2);
            material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;
        }
        
        public static void SetMaterialTransparent(this Material material, bool enabled)
        {
            material.SetFloat("_Surface", enabled ? MATERIAL_TRANSPARENT : MATERIAL_OPAQUE);
            material.SetShaderPassEnabled("SHADOWCASTER", !enabled);
            material.renderQueue = enabled ? 3000 : 2000;
            material.SetFloat("_DstBlend", enabled ? 10 : 0);
            material.SetFloat("_SrcBlend", enabled ? 5 : 1);
            material.SetFloat("_ZWrite", enabled ? 0 : 1);
        }
    }
}