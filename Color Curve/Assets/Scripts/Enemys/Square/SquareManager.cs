using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareManager : EnemyMovementAbstract
{
    [HideInInspector] public PlayerController _PlayerController;
    [SerializeField] private float _Speed;
    [SerializeField] private Transform BodyOne, BodyTwo;
    [SerializeField] private Rigidbody2D _Rb;
    private bool _canDash, _canFollow;

    public override void OnUpdate(AbstractmovementManager abstractmovementManager)
    {
        LookToPlayer();
    }

    public override void OnStart(AbstractmovementManager abstractmovementManager)
    {
        _canDash = true;
        _canFollow = true;
        _PlayerController = FindObjectOfType<PlayerController>();
    }

    public override void OnLateUpdate(AbstractmovementManager abstractmovementManager)
    {
        FollowCurrentPlayer();
    }

    private void LookToPlayer() => transform.up = _PlayerController.gameObject.transform.position - transform.position;
    private void FollowCurrentPlayer()
    {
        if (Vector2.Distance(transform.position, _PlayerController.gameObject.transform.position) > 5 && _canFollow)
        {
            TurnBody();
            transform.position = Vector3.MoveTowards(transform.position, _PlayerController.gameObject.transform.position, _Speed * Time.deltaTime);
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        else
        {
            if (!_canFollow)
            {
                if (_canDash)
                {
                    StartCoroutine(dashToPlayer());
                }
            }
        }
        if (Vector2.Distance(transform.position, _PlayerController.gameObject.transform.position) > 5)
        {
            _canDash = true;
            _canFollow = true;
        }
        else
        {
            _canFollow = false;
        }
    }

    private IEnumerator dashToPlayer()
    {
        _canDash = false;
        yield return new WaitForSeconds(.25f);
        _Rb.velocity = 30 * Time.deltaTime * 100 * transform.up;
        yield return new WaitForSeconds(.25f);
        _canFollow = true;
    }

    public override void TriggerEnter2D(AbstractmovementManager abstractmovementManager, Collider2D collision)
    {
    }
    private void TurnBody()
    {
        if (BodyOne != null)
            BodyOne.transform.Rotate(0, 0, 90 * Time.deltaTime * -2.5f);
        if(BodyTwo != null)
            BodyTwo.transform.Rotate(0, 0, 90 * Time.deltaTime * 2.5f);
    }
}
