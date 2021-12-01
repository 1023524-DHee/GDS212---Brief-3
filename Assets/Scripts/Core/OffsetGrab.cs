using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace HorrorVR.Core
{
    public class OffsetGrab : XRGrabInteractable
    {
        private Vector3 _interactorPosition = Vector3.zero;
        private Quaternion _interactorRotation = Quaternion.identity;

        protected override void OnSelectEntering(SelectEnterEventArgs args)
        {
            base.OnSelectEntering(args);
            StoreInteractor(args.interactor);
            MatchAttachmentPoints(args.interactor);
        }

        private void StoreInteractor(XRBaseInteractor interactor)
        {
            _interactorPosition = interactor.attachTransform.localPosition;
            _interactorRotation = interactor.attachTransform.localRotation;
        }

        private void MatchAttachmentPoints(XRBaseInteractor interactor)
        {
            bool hasAttach = attachTransform != null;
            interactor.attachTransform.position = hasAttach ? attachTransform.position : transform.position;
            interactor.attachTransform.rotation = hasAttach ? attachTransform.rotation : transform.rotation;
        }

        protected override void OnSelectExiting(SelectExitEventArgs args)
        {
            base.OnSelectExiting(args);
            ResetAttachmentPoint(args.interactor);
            ClearInteractor();
        }

        private void ResetAttachmentPoint(XRBaseInteractor interactor)
        {
            interactor.attachTransform.localPosition = _interactorPosition;
            interactor.attachTransform.localRotation = _interactorRotation;
        }

        private void ClearInteractor()
        {
            _interactorPosition = Vector3.zero;
            _interactorRotation = Quaternion.identity;
        }
    }
}
