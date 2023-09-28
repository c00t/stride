﻿using BepuPhysicIntegrationTest.Integration.Processors;
using BepuPhysics.Constraints;
using Stride.Core;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Engine.Design;

namespace BepuPhysicIntegrationTest.Integration.Components.Constraints
{
    [DataContract]
    [DefaultEntityComponentProcessor(typeof(ConstraintProcessor), ExecutionMode = ExecutionMode.Runtime)]
    [ComponentCategory("Bepu - Constraint")]
    public class AngularAxisGearMotorConstraintComponent : ConstraintComponent
    {
        public Vector3 LocalAxisA { get; set; }
        public float VelocityScale { get; set; }
        public MotorSettings Settings { get; set; }

    }
}
