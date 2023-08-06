using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject _BulletPrefab, _ParticlePrefab, _ComboEffect;
    [SerializeField, Range(0, 500)] private int _BulletSize;
    [SerializeField, Range(0, 500)] private int _ParticleSize;
    [SerializeField, Range(0, 500)] private int _ComboEffectSize;
    [SerializeField] private float _BulletSpeed;
    public Queue<GameObject> ParticleQueue = new Queue<GameObject>();
    public Queue<GameObject> BulletQueue = new Queue<GameObject>();
    public Queue<GameObject> ComboEffectQueue = new Queue<GameObject>();


    private List<Color> _randomTextColors = new List<Color>
    {
        Color.white,
        Color.red,
        Color.blue,
        Color.green,
    };
    void Start()
    {
        SpawnObjects(_BulletSize, BulletQueue, _BulletPrefab);
        SpawnObjects(_ParticleSize, ParticleQueue, _ParticlePrefab);
        SpawnObjects(_ComboEffectSize, ComboEffectQueue, _ComboEffect);
    }
    private void SpawnObjects(int size, Queue<GameObject> queue, GameObject prefab)
    {
        for (int i = 0; i < size; i++)
        {
            GameObject spawnedEffect = Instantiate(prefab);
            queue.Enqueue(spawnedEffect);
            spawnedEffect.SetActive(false);
        }
    }
    public GameObject TakeBullet(Vector3 playerPos)
    {
        GameObject takeBullet = BulletQueue.Dequeue();
        takeBullet.SetActive(true);
        takeBullet.transform.position = playerPos;
        AddVelocity(takeBullet.GetComponent<Rigidbody2D>(), playerPos);
        return takeBullet;
    }
    public void GiveBullet(GameObject bullet)
    {
        SetVelToDef(bullet.GetComponent<Rigidbody2D>());
        bullet.SetActive(false);
        BulletQueue.Enqueue(bullet);
    }
    private void AddVelocity(Rigidbody2D rb, Vector3 playerPos)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        rb.gameObject.transform.up = mousePos - playerPos;
        rb.velocity = rb.gameObject.transform.up * _BulletSpeed;
    }
    private void SetVelToDef(Rigidbody2D rb)
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
    }
    public GameObject TakeParticle(Vector2 pos)
    {
        GameObject taedComboEffect = ParticleQueue.Dequeue();
        taedComboEffect.SetActive(true);
        taedComboEffect.transform.position = pos;
        GiveParticle(taedComboEffect);
        return taedComboEffect;
    }
    public GameObject TakeComboEffect(Vector2 pos)
    {
        GameObject takedEffect = ComboEffectQueue.Dequeue();
        takedEffect.SetActive(true);
        takedEffect.GetComponent<Animator>().SetTrigger("combo");
        takedEffect.transform.GetChild(0).GetComponent<TMP_Text>().color = _randomTextColors[Random.Range(0, _randomTextColors.Count)];
        takedEffect.transform.position = new Vector2(pos.x + Random.Range(-.5f, .5f), pos.y + Random.Range(-.5f, .5f));
        GiveComboEffect(takedEffect);
        return takedEffect;
    }
    private void GiveParticle(GameObject particle)
    {
        ParticleQueue.Enqueue(particle);
    }
    private void GiveComboEffect(GameObject comboEffect)
    {
        ComboEffectQueue.Enqueue(comboEffect);
    }
}
