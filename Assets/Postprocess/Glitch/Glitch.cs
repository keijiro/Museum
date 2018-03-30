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
    }

    #endregion

    #region Effect renderer

    sealed class GlitchRenderer : PostProcessEffectRenderer<Glitch>
    {
        static class ShaderIDs
        {
            internal static readonly int Intensity = Shader.PropertyToID("_Intensity");
        }

        public override void Render(PostProcessRenderContext context)
        {
            var cmd = context.command;
            cmd.BeginSample("Glitch");

            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Museum/Glitch"));
            sheet.properties.SetFloat(ShaderIDs.Intensity, settings.intensity);

            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);

            cmd.EndSample("Glitch");
        }
    }

    #endregion
}
