using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableXP : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _MySpriteRenderer;
    [SerializeField] private List<Sprite> _ToSprite = new List<Sprite>();
    [SerializeField] List<Color> _Colors = new List<Color>();
    [SerializeField] private float _FromScale, _ToScale;
    [SerializeField] private float _FromRotation, _ToScaleRotation;

    private Transform _t;


    void Start()
    {
        _t = transform;
        int randNumSprte = Random.Range(0, _ToSprite.Count);
        int randNumColor = Random.Range(0, _Colors.Count);
        float randSize = Random.Range(_FromScale, _ToScale);
        float randRotation = Random.Range(_FromRotation, _ToScaleRotation);
        _MySpriteRenderer.sprite = _ToSprite[randNumSprte];
        _MySpriteRenderer.color = _Colors[randNumColor];
        _t.localScale = new Vector2(randSize, randSize);
        _t.rotation = Quaternion.Euler(0, 0, randRotation);
    }

}
