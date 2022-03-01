using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Collections;

public class IJobTest : MonoBehaviour {
    [SerializeField]
    private bool isJobOn;
    private void Update() {
        float timeStart = Time.realtimeSinceStartup;
        if (isJobOn) {
            NativeList<JobHandle> jobs = new NativeList<JobHandle>(Allocator.Temp);
            for (int i = 0; i < 10; i++) {
                jobs.Add(Get_MyTask_Job());
            }
            JobHandle.CompleteAll(jobs);
            jobs.Dispose();

        } else {
            // 10 times
            for (int i = 0; i < 10; i++) {
                MyTask();
            }
        }
        Debug.Log((Time.realtimeSinceStartup - timeStart) * 1000.0f + " ms.");
    }

    void MyTask() {
        float value = 0.0f;
        for (int i = 0; i < 50000; i++) {
            value = math.exp10(math.sqrt(value));
        }
    }

    JobHandle Get_MyTask_Job() {
        MyTask_Job myTask_Job = new MyTask_Job();
        return myTask_Job.Schedule();
    }

}

//[BurstCompile]
struct MyTask_Job : IJob {
    public void Execute() {
        float value = 0.0f;
        for (int i = 0; i < 50000; i++) {
            value = math.exp10(math.sqrt(value));
        }
    }
}
