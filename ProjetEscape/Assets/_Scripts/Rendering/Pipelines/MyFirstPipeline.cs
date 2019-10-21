using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName ="Rendering/Pipelines/My Pipeline")]
public class MyFirstPipeline : RenderPipelineAsset
{

    protected override RenderPipeline CreatePipeline()
    {
        return new OldConsolePipeline() {

        };
    }
    
}
