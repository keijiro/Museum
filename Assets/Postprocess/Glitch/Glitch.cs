using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Museum
{
    #region Effect settings

    [System.Serializable]
    [PostProcess(typeof(GlitchRenderer), PostProcessEvent.AfterStack, "Museum/Glitch")]
    public sealed class Glitch : PostProcessEffectSettings
    {
        [Range(0, 1)] public FloatParameter intensity = new FloatParameter { value = 0 };
        public TextureParameter overlay = new TextureParameter();
    }

    #endregion

    #region Effect renderer

    sealed class GlitchRenderer : PostProcessEffectRenderer<Glitch>
    {
        static class ShaderIDs
        {
            internal static readonly int Intensity = Shader.PropertyToID("_Intensity");
            internal static readonly int OverlayTex = Shader.PropertyToID("_OverlayTex");
        }

        public override void Render(PostProcessRenderContext context)
        {
            var cmd = context.command;
            cmd.BeginSample("Glitch");

            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Museum/Glitch"));
            sheet.properties.SetFloat(ShaderIDs.Intensity, settings.intensity);
            sheet.properties.SetTexture(ShaderIDs.OverlayTex, settings.overlay);

            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);

            cmd.EndSample("Glitch");
        }
    }

    #endregion
}
