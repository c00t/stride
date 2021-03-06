// Copyright (c) Stride contributors (https://stride3d.net) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core.Diagnostics;

namespace Stride.Core.BuildEngine
{
    public interface IBuildThreadMonitor
    {
        void Start();
        void RegisterBuildStep(BuildStep buildStep, TimestampLocalLogger stepLogger);
        void RegisterThread(int managedThreadId);
        void Finish();
        void Join();
    }
}
