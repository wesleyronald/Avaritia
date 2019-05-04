using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour 
{
    #region Private Variables
    //private Bomb bomb;
    private bool stopped = false, drawLineGizmo = false, canPushObjects = false;
    private BoxCollider2D boxCollider2D;
    private CircleCollider2D circleCollider2D;
    private Vector2 origin, dir, drawStart, drawDirection, colOffset;
    private Vector3 movement;
    private float horizontalSpeed, verticalSpeed, colSizeX, colSizeY, horizontalOffset, verticalOffset, horizontalMovement, verticalMovement;
    private RaycastHit2D hit;
    private string[] collidersTagArray;
    #endregion

    #region Gets & Sets
    public bool DrawLineGizmo()
    {
        return drawLineGizmo ;
    }

    public void DrawLineGizmo(bool drawLineGizmo)
    {
        this.drawLineGizmo = drawLineGizmo;
    }

    public bool CanPushObjects()
    {
        return canPushObjects ;
    }

    public void CanPushObjects(bool canPushObjects)
    {
        this.canPushObjects = canPushObjects;
    }

    public float HorizontalSpeed()
    {
        return horizontalSpeed;
    }

    public void HorizontalSpeed(float horizontalSpeed)
    {
        this.horizontalSpeed = horizontalSpeed;
    }

    public float VerticalSpeed()
    {
        return verticalSpeed;
    }

    public void VerticalSpeed(float verticalSpeed)
    {
        this.verticalSpeed = verticalSpeed;
    }

    public string[] CollidersTagArray()
    {
        return collidersTagArray;
    }

    public void CollidersTagArray(string[] collidersTagArray)
    {
        this.collidersTagArray = collidersTagArray;
    }

    public string CollidersTagArray(int tagIndex)
    {
        return collidersTagArray[tagIndex];
    }

    public void CollidersTagArray(int tagIndex, string tag)
    {
        collidersTagArray[tagIndex] = tag;
    }

    public Vector3 Movement()
    {
        return movement;
    }

    public void Stop()
    {
        stopped = true;
    }

    public void Move()
    {
        stopped = false;
    }

    public bool Stopped()
    {
        return stopped;
    }
    #endregion

    void Start () 
    {
        //bomb = GetComponent<Bomb>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        if (boxCollider2D)
        {

            colSizeX = boxCollider2D.size.x * transform.lossyScale.x;
            colSizeY = boxCollider2D.size.y * transform.lossyScale.y;
            colOffset = boxCollider2D.offset;
        } else if (circleCollider2D)
        {
            colSizeX = circleCollider2D.radius * 2 * transform.lossyScale.x;
            colSizeY = colSizeX * transform.lossyScale.y;
            colOffset = circleCollider2D.offset;
        }
        colOffset = new Vector2(colOffset.x * transform.lossyScale.x, colOffset.y * transform.lossyScale.y);
    }

    #region Horizontal only movement
    public void CalculateMovement(float horizontalPace)
    {
        if (!stopped)
        {
            CheckHits(horizontalPace, this.horizontalSpeed);

            movement = new Vector3(horizontalMovement, 0f, 0f);

            /*if (bomb)
            {
                stopped = (movement == Vector3.zero);
            }*/
            transform.position += movement;
        }
    }


    private void CheckHits(float horizontalPace, float horizontalSpeed)
    {
        horizontalOffset = (colSizeX / 2 * horizontalPace);
        horizontalMovement = horizontalPace * horizontalSpeed * Time.deltaTime;
        dir = new Vector2(transform.position.x + horizontalMovement, 0f);
        origin = new Vector2(transform.position.x + colOffset.x, transform.position.y + colOffset.y);
        hit = Physics2D.Raycast(origin, dir, horizontalOffset + horizontalMovement);
        CheckHorizontalHit();
    }
    #endregion

    #region 2D movement
    public void CalculateMovement(float horizontalPace, float verticalPace)
    {
        if (!stopped)
        {
            CheckHits(horizontalPace, verticalPace, this.horizontalSpeed, this.verticalSpeed);

            movement = new Vector3(horizontalMovement, verticalMovement, 0f);

            /*if (bomb)
            {
                stopped = (movement == Vector3.zero);
            }*/
            transform.position += movement;
        }
    }

    private void CheckHits(float horizontalPace, float verticalPace, float horizontalSpeed, float verticalSpeed)
    {
        if (horizontalPace != 0f)
        {
            horizontalOffset = (colSizeX / 2 * Mathf.Sign(horizontalPace));
        } else
        {
            horizontalOffset = 0f;
        }
        if (verticalPace != 0f)
        {
            verticalOffset = (colSizeY / 2 * Mathf.Sign(verticalPace));
        } else
        {
            verticalOffset = 0f;
        }
        horizontalMovement = horizontalPace * horizontalSpeed * Time.deltaTime;
        verticalMovement = verticalPace * verticalSpeed * Time.deltaTime;
        dir = new Vector2(transform.position.x + horizontalMovement, 0f);
        origin = new Vector2(transform.position.x + colOffset.x, transform.position.y - (colSizeY / 2) + 0.1f + colOffset.y);
        hit = Physics2D.Raycast(origin, dir, horizontalOffset + horizontalMovement);
        CheckHorizontalHit();
        origin = new Vector2(transform.position.x + colOffset.x, transform.position.y + colOffset.y);
        hit = Physics2D.Raycast(origin, dir, horizontalOffset + horizontalMovement);
        CheckHorizontalHit();
        origin = new Vector2(transform.position.x + colOffset.x, transform.position.y + (colSizeY / 2) - 0.1f + colOffset.y);
        hit = Physics2D.Raycast(origin, dir, horizontalOffset + horizontalMovement);
        CheckHorizontalHit();
        dir = new Vector2(0f, transform.position.y + verticalMovement);
        origin = new Vector2(transform.position.x - (colSizeX / 2) + 0.1f + colOffset.x, transform.position.y + colOffset.y);
        hit = Physics2D.Raycast(origin, dir, verticalOffset + verticalMovement);
        CheckVerticalHit();
        origin = new Vector2(transform.position.x + colOffset.x, transform.position.y + colOffset.y);
        hit = Physics2D.Raycast(origin, dir, verticalOffset + verticalMovement);
        CheckVerticalHit();
        origin = new Vector2(transform.position.x + (colSizeX / 2) - 0.1f + colOffset.x, transform.position.y + colOffset.y);
        hit = Physics2D.Raycast(origin, dir, verticalOffset + verticalMovement);
        CheckVerticalHit();
    }
    #endregion

    #region Movement calculation
    private void CheckHorizontalHit()
    {
        if (hit.collider != null && IsSolid(hit))
        {
            /*if (canPushObjects && hit.collider.gameObject.GetComponent<Pushable>())
            {
                hit.collider.gameObject.GetComponent<Pushable>().SetHorizontalPush(horizontalPace);
                hit.collider.gameObject.GetComponent<Pushable>().Push();
            }*/
            if (hit.point.x - transform.position.x - horizontalOffset - colOffset.x != 0f)
            {
                horizontalMovement = hit.point.x - transform.position.x - horizontalOffset - colOffset.x;
            }
            else
            {

                horizontalMovement = 0f;
            } 
        }
    }

    private void CheckVerticalHit()
    {
        if (hit.collider != null && IsSolid(hit))
        {
            /*if (canPushObjects && hit.collider.gameObject.GetComponent<Pushable>())
            {
                hit.collider.gameObject.GetComponent<Pushable>().SetVerticalPush(verticalPace);
                hit.collider.gameObject.GetComponent<Pushable>().Push();
            }*/
            if (hit.point.y - transform.position.y - verticalOffset - colOffset.y != 0f)
            {
                verticalMovement = hit.point.y - transform.position.y - verticalOffset - colOffset.y;
            }
            else
            {
                verticalMovement = 0f;
            }
        }
    }
    #endregion

    public bool IsSolid(RaycastHit2D ray)
    {
        foreach (string colliderTag in collidersTagArray)
        {
            if (ray.collider.gameObject.CompareTag(colliderTag))
            {
                return true;
            }
        }
        return false;
    }

    public void CalculatePush(float horizontalPace, float verticalPace, float pushSpeed)
    {
        CheckHits(horizontalPace, verticalPace, pushSpeed, pushSpeed);
        movement = new Vector3(horizontalMovement, verticalMovement, 0f);

        transform.position += movement;
    }

    #region Ceiling, ground and edge checks
    public bool IsHittingCeiling()
    {
        Vector2 dir = Vector2.up;
        Vector2 origin = new Vector2(transform.position.x - (colSizeX / 2) + 0.1f + colOffset.x, transform.position.y + colOffset.y);
        hit = Physics2D.Raycast(origin, dir, colSizeY /2);
        if (hit.collider != null && IsSolid(hit))
        {
            return true;
        }
        origin = new Vector2(transform.position.x + colOffset.x, transform.position.y + colOffset.y);
        hit = Physics2D.Raycast(origin, dir, colSizeY /2);
        if (hit.collider != null && IsSolid(hit))
        {
            return true;
        }
        origin = new Vector2(transform.position.x + (colSizeX / 2) - 0.1f + colOffset.x, transform.position.y + colOffset.y);
        hit = Physics2D.Raycast(origin, dir, colSizeY /2);
        if (hit.collider != null && IsSolid(hit))
        {
            return true;
        }
        return false;
    }

    public bool IsOnTheGround ()
    {
        LayerMask layermask = 1 << 0;
        layermask |= 1 << 2;
        Vector2 dir = Vector2.down;
        Vector2 origin = new Vector2(transform.position.x - (colSizeX / 2) + 0.001f + colOffset.x, transform.position.y + colOffset.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, 0.001f + colSizeY /2, layermask);
        if (hit.collider != null && IsSolid(hit))
        {
            return true;
        }
        origin = new Vector2(transform.position.x + colOffset.x, transform.position.y + colOffset.y);
        hit = Physics2D.Raycast(origin, dir, 0.001f + colSizeY /2, layermask);
        if (hit.collider != null && IsSolid(hit))
        {
            return true;
        }
        origin = new Vector2(transform.position.x + (colSizeX / 2) - 0.001f + colOffset.x, transform.position.y + colOffset.y);
        hit = Physics2D.Raycast(origin, dir, 0.001f + colSizeY /2, layermask);
        if (hit.collider != null && IsSolid(hit))
        {
            return true;
        }
        return false;
    }

    public bool EnemyNearEdge(bool flipX)
    {
        float direction = (colSizeX / 2);
        if (!flipX)
        {
            direction = -direction;
        }
        Vector2 dir = Vector2.down;
        Vector2 origin = new Vector2(transform.position.x + direction + colOffset.x, transform.position.y + colOffset.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, colSizeY /2 + 0.5f);
        if (hit.collider != null && IsSolid(hit))
        {
            return false;
        }
        return true;
    }
    #endregion

    public void DrawLines()
    {
        drawStart = new Vector2(transform.position.x + colOffset.x - (colSizeX / 2) - 0.05f, transform.position.y + colOffset.y + (colSizeY / 2) - 0.1f);
        drawDirection = new Vector2(drawStart.x + (colSizeX) + 0.1f, drawStart.y);
        Debug.DrawLine(drawStart, drawDirection, Color.blue);
        drawStart = new Vector2(transform.position.x + colOffset.x - (colSizeX / 2) - 0.05f, transform.position.y + colOffset.y);
        drawDirection = new Vector2(drawStart.x + (colSizeX) + 0.1f, drawStart.y);
        Debug.DrawLine(drawStart, drawDirection, Color.blue);
        drawStart = new Vector2(transform.position.x + colOffset.x - (colSizeX / 2) - 0.05f, transform.position.y + colOffset.y - (colSizeY / 2) + 0.1f);
        drawDirection = new Vector2(drawStart.x + (colSizeX) + 0.1f, drawStart.y);
        Debug.DrawLine(drawStart, drawDirection, Color.blue);

        drawStart = new Vector2(transform.position.x + colOffset.x - (colSizeX / 2) + 0.1f, transform.position.y + colOffset.y - (colSizeY / 2) - 0.05f);
        drawDirection = new Vector2(drawStart.x , drawStart.y + (colSizeY) + 0.1f);
        Debug.DrawLine(drawStart, drawDirection, Color.blue);
        drawStart = new Vector2(transform.position.x + colOffset.x, transform.position.y + colOffset.y - (colSizeY / 2) - 0.05f);
        drawDirection = new Vector2(drawStart.x , drawStart.y + (colSizeY) + 0.1f);
        Debug.DrawLine(drawStart, drawDirection, Color.blue);
        drawStart = new Vector2(transform.position.x + colOffset.x + (colSizeX / 2) - 0.1f, transform.position.y + colOffset.y - (colSizeY / 2) - 0.05f);
        drawDirection = new Vector2(drawStart.x , drawStart.y + (colSizeY) + 0.1f);
        Debug.DrawLine(drawStart, drawDirection, Color.blue);
    }
}