mergeInto(LibraryManager.library, {
  SetFinalScore: function (fScore) {
      console.log(Math.floor(fScore));
      window.dispatchReactUnityEvent("FinalScoreEvent", Math.floor(fScore));
  },
  SetPlayerName: function (playerNameSet) {
      console.log(playerNameSet)
      window.dispatchReactUnityEvent("PlayerNameEvent", Pointer_stringify(playerNameSet));
  },
  SetReady: function (isReady) {
      console.log(isReady)
      window.dispatchReactUnityEvent("IsReadyEvent", Pointer_stringify(isReady));
  },
  SetHeartNumber: function (heartNumber) {
      console.log(heartNumber)
      window.dispatchReactUnityEvent("HeartEvent", Math.floor(heartNumber));
  },
  SetSpaceClicked: function (countValue) {
      window.dispatchReactUnityEvent("Space", Math.floor(countValue));
  }
});