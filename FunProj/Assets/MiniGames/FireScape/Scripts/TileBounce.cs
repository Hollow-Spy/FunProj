using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class TileBounce : MonoBehaviour
{
    Tilemap tilemap;
    [SerializeField] LayerMask whatisground;
    [SerializeField] LayerMask whatisPlayer;
    [SerializeField] GameObject HitSound;
    GameObject empty;

    Vector2 gizmopos;

    private void Start()
    {
         empty = new GameObject();
        tilemap = GetComponent<Tilemap>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            RaycastHit2D hit = Physics2D.Raycast(collision.gameObject.transform.position, Vector2.up * .5f);
           
            if (hit.collider != null)
            {
              

                Vector3Int position = tilemap.WorldToCell(new Vector2(collision.contacts[0].point.x, collision.contacts[0].point.y + 0.097f));
                Vector2 tilepos = tilemap.GetCellCenterWorld(position);

                TileBase tile = tilemap.GetTile(position);
                
                if (tile != null)
                {
                    GameObject ShakeTile = Instantiate(empty,tilepos,Quaternion.identity );
                    ShakeTile.AddComponent<SpriteRenderer>();
                    ShakeTile.GetComponent<SpriteRenderer>().sprite = tilemap.GetSprite(position);
                    ShakeTile.AddComponent<BoxCollider2D>();
                    ShakeTile.GetComponent<BoxCollider2D>().size = new Vector2(.2f, .2f);
                    ShakeTile.GetComponent<SpriteRenderer>().sortingOrder = 3;

                    tilemap.SetTile(position, null);

                    Instantiate(HitSound, position, Quaternion.identity);

                    gizmopos = new Vector2(tilepos.x, tilepos.y + 0.097f);
                    if (Physics2D.OverlapCircle(new Vector2(tilepos.x, tilepos.y + 0.097f ) , .2f ,whatisPlayer))
                    {
                        Collider2D[] hitcollider = Physics2D.OverlapCircleAll(new Vector2(tilepos.x, tilepos.y + 0.097f), .5f, whatisPlayer);

                        


                        for (int i = 0;i<hitcollider.Length;i++)
                        {
                            if(hitcollider[i].transform.position.y > tilepos.y -.1f)
                            {
                                hitcollider[i].gameObject.GetComponent<PlayerController>().Knockout(new Vector2(Random.Range(-1, 1), 2));
                            }
                          
                        }

                    }

                        StartCoroutine( ShakingNumerator(position,tile,ShakeTile) );

                }
            }




        }



    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 0, 0.75F);
        Gizmos.DrawSphere(gizmopos, .2f);
    }

    IEnumerator ShakingNumerator(Vector3Int position, TileBase tile, GameObject shaketile )
    {
        Vector2 ogPos = shaketile.transform.position;

        for(int i=0;i<4;i++)
        {
            shaketile.transform.position = new Vector2(shaketile.transform.position.x + Random.Range(-.02f, .02f) ,shaketile.transform.position.y + Random.Range(-.02f,.02f));
            yield return new WaitForSeconds(.03f);
            shaketile.transform.position = ogPos;
            yield return new WaitForSeconds(.03f);


        }
        tilemap.SetTile(position, tile);
        Destroy(shaketile);

       
    }


   
}
