using UnityEngine;
using Microsoft.MixedReality.QR;
using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit;
using System.Threading;
using System.Threading.Tasks;

public class QrScanner : MonoBehaviour
{
    
    private QRCodeWatcher qrCodeWatcher;
    [SerializeField] private List<Target> qrRecenterPositions = new List<Target>() ;
    [SerializeField] private GameObject MixedRealityPlaySpace;
    private string QrData = ""; 
    
    async void Start()
    {
        var accessStatus = await QRCodeWatcher.RequestAccessAsync();
        if (accessStatus != QRCodeWatcherAccessStatus.Allowed){
            Debug.LogWarning("User has not granted permission to detect QR codes.");
            return;
        }
        qrCodeWatcher = new QRCodeWatcher();
        qrCodeWatcher.Updated += QRCodeUpdated;
        // qrCodeWatcher.Added += QRCodeAdded;
        // qrCodeWatcher.Removed += QRCodeRemoved;
        qrCodeWatcher.Start();
        
    }

    async void QRCodeUpdated(object sender, QRCodeUpdatedEventArgs args){
        QrData = args.Code.Data;
        if (QrData != null){
            MainThreadDispatcher.Instance.ExecuteOnMainThread(() =>{
            RecenterCamera(QrData);
        });
    }
    }
    private void RecenterCamera(string target){
        Target currentLocation = qrRecenterPositions.Find(x=>x.Name.Equals(QrData, StringComparison.OrdinalIgnoreCase));    
        if (currentLocation !=null ){
            MixedRealityPlaySpace.transform.position =  currentLocation.PositionObject.transform.position;
            MixedRealityPlaySpace.transform.rotation = currentLocation.PositionObject.transform.rotation;
        }else{
            Debug.Log("Current location is not found!");
        }
        
    }
}

    // private void QRCodeAdded(object sender, QRCodeAddedEventArgs args){
    //     Debug.Log($"QR Code Added: Data = {args.Code.Data}, Version = {args.Code.Version}");
    // }
    // void OnDisable()
    // {
    //     // Stop the QRCodeWatcher when the script is disabled or the app is closed.
    //     if (qrCodeWatcher != null)
    //     {
    //         qrCodeWatcher.Stop();
    //         qrCodeWatcher = null;
    //     }
    // }
    // private void QRCodeRemoved(object sender, QRCodeRemovedEventArgs args)
    // {
    //     // Handle the removed QR code. Access the QR code data through args.Code.
    //     Debug.Log($"QR Code Removed: Data = {args.Code.Data}, Version = {args.Code.Version}");
    // }