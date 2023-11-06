mergeInto(LibraryManager.library, {
  SetPlayerName: function (playerNameSet) {
      window.dispatchReactUnityEvent("PlayerNameEvent", Pointer_stringify(playerNameSet));
  },
  SetReadyState: function (readyState) {
      window.dispatchReactUnityEvent("ReadyStateEvent", Pointer_stringify(readyState));
  },
  SetPlayerScore: function (playerScore) {
      window.dispatchReactUnityEvent("PlayerScoreEvent", Math.floor(playerScore));
  },
  SetRemainingChance: function (remainingChance) {
      window.dispatchReactUnityEvent("RemainingChanceEvent", Math.floor(remainingChance));
  },
  SetHoldingTime: function (holdingTime) {
      window.dispatchReactUnityEvent("HoldingTimeEvent", Math.floor(holdingTime));
  }
});