﻿using System;
using System.Collections.Generic;
using System.Text;
using Stride.Core;
using Stride.Shaders;
using Stride.Rendering.Materials;

namespace Stride.Rendering.Voxels
{
    [DataContract(DefaultMemberMode = DataMemberMode.Default)]
    [Display("R11G11B10F")]
    public class VoxelFragmentPackFloatR11G11B10 : IVoxelFragmentPacker
    {
        ShaderSource source = new ShaderClassSource("VoxelFragmentPackFloatR11G11B10");
        public ShaderSource GetShader()
        {
            return source;
        }
        public int GetBits(int channels)
        {
            return ((channels+2) / 3)*32;//(channels)/3 * 32 + (channels % 3) * 11;
        }
    }
}
