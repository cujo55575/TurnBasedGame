using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour
{
    Vector3 offset;

    [SerializeField] Transform parentCell;
    BoxCollider _collider;

    string destinationTag = "DropArea";

    public Action onClick = null;
    public Action<ShopCell> onMove = null;
    public Action<ShopCell> onBuy = null;
    public static Action onCheckingLeveledUpAfterBuy;
    public static Action onNotEnoughCoin;
    public static Action onDrag;
    public static Action<bool, ShopCell> onMoving;
    public static Action<ShopCell> onRayCastHits;

    public Transform ParentCell
    {
        get { return parentCell; }
        set { parentCell = value; }
    }

    private void Start()
    {
        _collider = GetComponent<BoxCollider>();
        _collider.size = new Vector3(5, 5, 5);

        parentCell = transform.parent;

        // Hide Model's collider to prevent raycast OnMouseUp
        HideModelCollider();
    }

    private void OnMouseDown()
    {
        offset = transform.position - GetMousePosition();
        _collider.enabled = false;

        onClick?.Invoke();
    }

    private void OnMouseDrag()
    {
        transform.position = GetMousePosition() + offset;

        ShopCell shopCellThis = transform.parent.GetComponent<ShopCell>();

        if (shopCellThis)
        {
            onMoving?.Invoke(true, shopCellThis);

            if (shopCellThis.IsBoardCell)
            {
                if (transform.localPosition.x >= 10 || transform.localPosition.x <= -10)
                {
                    onDrag?.Invoke();
                }
            }
        }

        Vector3 rayOrigin = Camera.main.transform.position;
        Vector3 rayDirection = GetMousePosition() - rayOrigin;

        RaycastHit[] hits = Physics.RaycastAll(rayOrigin, rayDirection);
        if (hits.Length > 0)
        {
            if (hits[0].transform.tag == destinationTag)
            {
                onRayCastHits?.Invoke(hits[0].transform.GetComponent<ShopCell>());
            }
            else if (hits[0].transform.parent.tag == destinationTag)
            {
                onRayCastHits?.Invoke(hits[0].transform.parent.GetComponent<ShopCell>());
            }
        }
    }

    private void OnMouseUp()
    {
        //Current Cell has Emoji
        ShopCell shopCellThis = transform.parent.GetComponent<ShopCell>();

        if (shopCellThis)
        {
            onMoving?.Invoke(false, shopCellThis);

            Vector3 rayOrigin = Camera.main.transform.position;
            Vector3 rayDirection = GetMousePosition() - rayOrigin;

            RaycastHit[] hits = Physics.RaycastAll(rayOrigin, rayDirection);
            if (hits.Length > 0)
            {
                //If hits Board Cell
                if (hits[0].transform.tag == destinationTag)
                {
                    ShopCell shopCellHit = hits[0].transform.GetComponent<ShopCell>();
                    //If Board Cell has Emoji
                    if (shopCellHit.shopEmoji != null)
                    {
                        //Check you Emoji could be merged with Emoji in Board Cell
                        bool checkMerge = shopCellHit.OnCheckingLevelUp(shopCellThis);
                        if (!checkMerge)
                        {
                            if (shopCellHit.emojiData.baseEmojiId.CompareTo(shopCellThis.emojiData.baseEmojiId) == 0)
                            {
                                BackToPreviousPlace();
                            }
                            else if (!shopCellThis.IsBoardCell)
                            {
                                BackToPreviousPlace();
                            }
                            else
                            {
                                if (shopCellHit == shopCellThis)
                                {
                                    BackToPreviousPlace();
                                }
                                else
                                {
                                    //If can't merge, let swap position
                                    SwapEmoji(shopCellHit.shopEmoji.transform);
                                    onMove?.Invoke(shopCellThis);
                                }
                            }
                        }
                    }

                    //If Board Cell hasn't Emoji, means the Cell is avalaible, can Stay here
                    else
                    {
                        PlayerData player = DataController.instance.currentPlayer;
                        if (player.gameState.coins < shopCellThis.emojiData.cost && !shopCellThis.IsBoardCell)
                        {
                            onNotEnoughCoin?.Invoke();
                            BackToPreviousPlace();
                        }
                        else
                        {
                            onBuy?.Invoke(shopCellHit);
                            MoveEmojiToNewPos(shopCellHit);
                            onCheckingLeveledUpAfterBuy?.Invoke();
                        }
                    }
                }
                //If hits Emoji
                else if (hits[0].transform.tag == transform.tag)
                {
                    //If the parent of this Emoji is Board Cell
                    if (hits[0].transform.parent.tag == destinationTag)
                    {
                        ShopCell shopCellHit = hits[0].transform.parent.GetComponent<ShopCell>();

                        //Check you Emoji could be merged with Emoji in Board Cell
                        bool checkMerge = shopCellHit.OnCheckingLevelUp(shopCellThis);
                        if (!checkMerge)
                        {
                            if (shopCellHit.emojiData.baseEmojiId.CompareTo(shopCellThis.emojiData.baseEmojiId) == 0)
                            {
                                BackToPreviousPlace();
                            }
                            else if (!shopCellThis.IsBoardCell)
                            {
                                BackToPreviousPlace();
                            }
                            else
                            {
                                if (shopCellHit == shopCellThis)
                                {
                                    BackToPreviousPlace();
                                }
                                else
                                {
                                    //If can't merge, let swap position
                                    SwapEmoji(shopCellHit.shopEmoji.transform);
                                    onMove?.Invoke(shopCellThis);
                                }
                            }
                        }
                    }
                    //If the parent of this Emoji is Shop cell, do nothing
                    else
                    {
                        BackToPreviousPlace();
                    }
                }
                //If not hits Emoji or Cell, do nothing
                else
                {
                    BackToPreviousPlace();
                }
            }
            //If hits nothing, do nothing
            else
            {
                BackToPreviousPlace();
            }
        }
    }

    private void BackToPreviousPlace()
    {
        _collider.enabled = true;
        transform.parent = parentCell;
        transform.localPosition = new Vector3(0, 9, 0);
    }

    private Vector3 GetMousePosition()
    {
        Vector3 mousePositon = Input.mousePosition;
        mousePositon.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mousePositon);
    }

    private void HideModelCollider()
    {
        Transform model = transform.GetChild(0).GetChild(0);

        BoxCollider collider = model.GetComponent<BoxCollider>();

        if (collider) collider.enabled = false;
    }

    private void SwapEmoji(Transform emoji)
    {
        //Select emojiData
        var selectEmojiData = transform.parent.GetComponent<ShopCell>().emojiData;
        var targetEmojiData = emoji.transform.parent.GetComponent<ShopCell>().emojiData;

        //Select
        transform.parent = emoji.transform.parent;
        transform.localPosition = Vector3.zero;

        //Target To Swap
        emoji.parent = parentCell;
        emoji.GetComponent<DragDrop>().ParentCell = parentCell;
        emoji.localPosition = Vector3.zero;

        _collider.enabled = true;
        parentCell = transform.parent;

        transform.parent.GetComponent<ShopCell>().Setup(selectEmojiData);
        emoji.parent.GetComponent<ShopCell>().Setup(targetEmojiData);

        foreach (var item in DataController.instance.currentPlayer.board.placements)
        {
            if (transform.parent.GetComponent<ShopCell>().emojiData.emojiId == item.emojiId)
            {
                item.index = transform.parent.GetComponent<ShopCell>().coord;
            }
            if (emoji.parent.GetComponent<ShopCell>().emojiData.emojiId == item.emojiId)
            {
                item.index = emoji.parent.GetComponent<ShopCell>().coord;
            }
        }
    }

    private void MoveEmojiToNewPos(ShopCell cell)
    {
        var selectShopCell = transform.parent.GetComponent<ShopCell>();

        transform.parent = cell.transform;
        transform.localPosition = Vector3.zero;

        _collider.enabled = true;
        parentCell = transform.parent;

        transform.parent.GetComponent<ShopCell>().Setup(selectShopCell.emojiData);

        foreach (var item in DataController.instance.currentPlayer.board.placements)
        {
            if (cell.emojiData.emojiId == item.emojiId)
            {
                item.index = transform.parent.GetComponent<ShopCell>().coord;
            }
        }

        selectShopCell.Setup();
    }
}
