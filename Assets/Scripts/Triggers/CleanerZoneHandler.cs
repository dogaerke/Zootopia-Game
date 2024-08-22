using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DefaultNamespace;
using ZoneStatusNamespace;

public class CleanerZoneHandler : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] [Range(0.01f, 0.1f)] private float speed = 0.01f;
    [SerializeField] private float pulse = 0.1f;
    [SerializeField] private RoomController room;
    [SerializeField] private ZoneStatus status;
    [SerializeField] private int areaNumber;
    
    private bool _isCleaned;
    private Tween _timeTween;
    private Coroutine _newRoutine;
    [SerializeField] private GameObject _cleaningMaterial;

    public bool IsCleaned
    {
        get => _isCleaned;
        set => _isCleaned = value;
    }
    
    public ZoneStatus Status
    {
        get => status;
        set => status = value;
    }

    private void OnEnable()
    {
        HelperManager.Instance.RegisterCleanZoneDict(areaNumber, this);
    }

    private void OnDisable()
    {
        HelperManager.Instance.RemoveCleanZone(areaNumber, this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Helper"))
        {
            _newRoutine = StartCoroutine(ProcessFill());

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Helper"))
        {
            ProcessExit();
        }
    }

    IEnumerator ProcessFill()
    {
        do
        {
            var currentFillAmount = fillImage.fillAmount;
            var destination = currentFillAmount;
            // _haveEmptyRoom = RoomOccupancyController.Instance.TryGetEmptyRoom(out var room);
            // _inLineCount = CustomersManager.Instance.customerInLineList.Count;
            if (!IsCleaned && room.status == RoomStatus.Unavailable)
            {
                destination = Mathf.Clamp(destination + speed, 0f, 1f);

                _timeTween?.Kill();
                _timeTween = DOVirtual.Float(currentFillAmount, destination, pulse, (value) =>
                {
                    fillImage.fillAmount = value;
                });

                if (destination >= 1f) //When filling complete
                {
                    //animation
                    _isCleaned = true;
                    _cleaningMaterial.SetActive(false);

                    if (room.AreZonesCleaned())
                    {
                        room.isDirty = false;
                        PlayerPrefs.SetInt(room.name + "Dirty", 0);
                        
                        if (room.IsClaimFood())
                        {
                            room.RoomAvailable();
                        }
                    }
                    
                    fillImage.fillAmount = 0f;
                    _timeTween.Kill();
                    
                    
                    gameObject.SetActive(false);
                    
                }
            }
            yield return new WaitForSeconds(pulse);

        } while (RoomsManager.Instance.roomList != null);
        
    }
    public void ChangeStatus(ZoneStatus target)
    {
        status = target;
        switch (target)
        {
            case ZoneStatus.Available:
                break;
            case ZoneStatus.Unavailable:
                break;
            default:
                return;
        }
    }

    private void ProcessExit()
    {
        if(_newRoutine != null)
            StopCoroutine(_newRoutine);
        fillImage.fillAmount = 0;
        _timeTween?.Kill();
    }

    

}
