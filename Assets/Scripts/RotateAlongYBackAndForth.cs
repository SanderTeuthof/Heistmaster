using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RotateAlongYBackAndForth : MonoBehaviour
{
    [SerializeField]
    private float _addRotation;
    [SerializeField]
    private float _rotateSpeed;

    private bool _enabled = false;
    private Queue<float> _rotationQueue = new Queue<float>();

    private bool _rotated = false;

    public void RotateRotation()
    {
        if (_rotated)
        {
            _rotationQueue.Enqueue(_addRotation*-1);
            _rotated = false;
        } 
        else
        {
            _rotationQueue.Enqueue(_addRotation);
            _rotated = true;
        }
        

        if (!_enabled)
        {
            StartCoroutine(Rotate());
        }
    }

    private IEnumerator Rotate()
    {
        _enabled = true;

        while (_rotationQueue.Count > 0)
        {
            float rotation = _rotationQueue.Dequeue();
            Quaternion startRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.Euler(0f, startRotation.eulerAngles.y + rotation, 0f);

            float elapsedTime = 0f;

            while (elapsedTime < 1f)
            {
                transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime);
                elapsedTime += Time.deltaTime * _rotateSpeed;
                yield return null;
            }

            transform.rotation = targetRotation;
            yield return null; // Optional delay between rotations

            // If there is another rotation in the queue, wait for a short delay
            if (_rotationQueue.Count > 0)
            {
                yield return new WaitForSeconds(0.1f); // Adjust the delay as needed
            }
        }

        _enabled = false;
    }
}
