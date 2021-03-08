using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubbyBoy : MonoBehaviour
{
    private TaskManager _tm = new TaskManager();

    public float speed, rotationSpeed;
    private Vector3 spinAxis;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            timeSpinning = 0.0f;

            var jitterTimer = 0f;
            var moveAtMouse = new MoveTowardsMouseForSeconds(this, 2.0f);
            var spin = new DelegateTask(PickSpinRotation, SpinForSeconds);
            var jitter = new DelegateTask(() => {}, () =>
            {
                jitterTimer += Time.deltaTime;
                
                var position = transform.position + 0.1f * Random.insideUnitSphere;
                position.z = 0;
                transform.position = position;
                
                return jitterTimer > 3;
            });

            moveAtMouse.Then(spin).Then(jitter);

            _tm.Do(moveAtMouse);
        }

        _tm.Update();
    }

    float timeSpinning = 0.0f;
    private bool SpinForSeconds()
    {
        Spin();
        timeSpinning += Time.deltaTime;
        return timeSpinning > 4;
    }
    
    private void MoveTowardsMouse()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var moveTowards = (mousePosition - transform.position).normalized;
        moveTowards.z = 0;

        transform.position += speed * Time.deltaTime * moveTowards;
    }

    private void PickSpinRotation()
    {
        spinAxis = Random.insideUnitSphere;
    }

    private void Spin()
    {
        transform.Rotate(spinAxis, rotationSpeed * Time.deltaTime);
    }

    private class MoveTowardsMouseForSeconds : Task
    {
        private float duration;
        private CubbyBoy toMove;
        private float elapsedTime = 0;
        
        public MoveTowardsMouseForSeconds(CubbyBoy toMove, float duration)
        {
            this.duration = duration;
            this.toMove = toMove;
        }

        internal override void Update()
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= duration)
            {
                SetStatus(TaskStatus.Success);
            }

            toMove.MoveTowardsMouse();
        }

        protected override void OnSuccess()
        {
            Debug.Log("ONSUCCESS");
        }
    }
}
