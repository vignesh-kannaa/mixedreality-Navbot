using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System.Threading.Tasks;

public class SetNavigationTarget: MonoBehaviour {

  
  [SerializeField] private GameObject guide; // agent
  [SerializeField] private Animator anim; // to control the animation of the guide  
  [SerializeField] private TMP_Dropdown navigationTargetDropDown;//drop down for targets
  [SerializeField] private List < Target > navigationTargetObjects = new List < Target > ();
  [SerializeField] private SpeechInput speechInput;
  
  public AudioMsg audioMsg;
  private NavMeshPath path;
  private LineRenderer line;
  private Vector3 targetPosition;
  private Vector3 currentPosition;

  // initialising values
  private int cornerIndex = 0;
  private float moveSpeed = 0.3f; // Adjust this value to control the speed of the guide.

  // creating flags
  private bool targetReached = false; // to run the fucntion only once
  private bool targetSelected = false;
  private bool finishReached = false;
  private bool startedFlag = true;

  // runs only once at the start
  async void Start() {
    path = new NavMeshPath();
    line = transform.GetComponent < LineRenderer > ();
    // SetCurrentNavigationTarget(2);
    // await Task.Delay(3000);
    // setGuide();
  }

  // runs everytime 
  private void Update() {
    if (targetSelected && !targetReached) {
      move();
    }
    if (!finishReached && targetReached) {
      reached();
    }
  }

   async public void setGuide() {
    if (startedFlag ==  true){
      startedFlag = false;
      await Task.Delay(3000);
      // guide.transform.position = transform.position + (2f * transform.forward); // place the guide in same position of the user
      await audioMsg.PlayAudio(selectMsg(Constants.WelcomeMessages));
      setRotation(transform.position , guide.transform.position);
    }
  }

  // void OnButtonClick2() {
  //   Debug.Log("Button clicked!");
  //   // SetCurrentNavigationTarget(2);
  // }

  void move() {
    NavMesh.CalculatePath(guide.transform.position, targetPosition, NavMesh.AllAreas, path);
    line.positionCount = path.corners.Length;
    line.SetPositions(path.corners);
    line.enabled = true; //set the line size to 1 in line rendered property to hide it
    float distance = Vector3.Distance(guide.transform.position, transform.position); // distance bt user and guide
    if (distance >= 5f) {
      anim.SetFloat("Action", 0f); // Set guide's action to idle
      // await audioMsg.PlayAudio(selectMsg(Constants.WaitingMessages));
      setRotation(transform.position, guide.transform.position);
    } else {
      anim.SetFloat("Action", 1f); // action 1 specifies walking
      setRotation(targetPosition, guide.transform.position);
      if (cornerIndex < path.corners.Length) {
        // Move the guide towards the next corner in the path.
        guide.transform.position = Vector3.MoveTowards(guide.transform.position, path.corners[cornerIndex], moveSpeed * Time.deltaTime);
        if (Vector3.Distance(guide.transform.position, path.corners[cornerIndex]) < 0.5f) {
          // If the guide is close enough to the current corner, move to the next one.
          cornerIndex++;
          if (cornerIndex < path.corners.Length) {
            targetPosition = path.corners[cornerIndex];
          }
        }
      } else {
        targetReached = true;
      }
    }
  }

  async void reached() {
    finishReached = true;
    setRotation(transform.position, guide.transform.position);
    await audioMsg.PlayAudio(selectMsg(Constants.DestinationMessages));
  }  
  
  async public void SetCurrentNavigationTarget(int selectedValue) {
    targetPosition = Vector3.zero;
    string selectedText = navigationTargetDropDown.options[selectedValue].text;
    Target currentTarget = navigationTargetObjects.Find(x => x.Name.Equals(selectedText));
    Debug.Log("Current destination:" + currentTarget.Name);
    if (currentTarget != null) {
      await audioMsg.PlayAudio(selectMsg(Constants.FollowMeMessages));
      targetPosition = currentTarget.PositionObject.transform.position;
      setActionFlags();
      setRotation(targetPosition, guide.transform.position);
    }
  }
  
  void setActionFlags() {
    //reseting flags so that guide can move from one place to another
    cornerIndex = 0;
    targetSelected = true;
    finishReached = false;
    targetReached = false;
  }
  
  //to move indicator and agent to that location
  // use this function to test in UI game mode
  public void SetCurrentLocation(int selectedValue) {
    Debug.Log("Current destination:" + selectedValue);
    // qrScanner.recenterCamera("R214");
  }
  // helper functions
  private string selectMsg(List < string > messageList) {
    int randomIndex = Random.Range(0, messageList.Count);
    string message = messageList[randomIndex];
    return message;
  }
  void setRotation(Vector3 targetPosition, Vector3 guidePosition) {
    Vector3 direction = targetPosition - guidePosition; // get direction from transform to guide
    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
    Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
    guide.transform.rotation = Quaternion.Slerp(guide.transform.rotation, targetRotation, 3f * Time.deltaTime);
  }

}

/*
1. first target is selected in the UI and triggers SetCurrentNavigationTarget(), 
2. then targetselected flag is set to true to call function move()
3. once reached to final destination, reached function is called
4. To avoid looping of reached func, flag is made to stop
5. all flags are resetted when other target is selected*/

// public void SetCurrentNavigationTarget(int selectedValue) {
//     targetPosition = Vector3.zero;
//     string selectedText = navigationTargetDropDown.options[selectedValue].text;
//     Target currentTarget = navigationTargetObjects.Find (x => x.Name.Equals(selectedText));

//     if (currentTarget != null) {
//         targetPosition = currentTarget.PositionObject.transform.position;
//         setActionFlags();
//         floatingText.setPopMessage(Constants.FollowMeMsg);
//         setRotation(targetPosition,guide.transform.position);
//         // Debug.Log("Target position set to: " + targetPosition);
//         }
//     // } else {
//     //     Debug.LogWarning("Current target is null.");
//     // }
// }