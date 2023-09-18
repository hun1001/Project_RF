using Addressable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayer : Player
{
    protected override IEnumerator InputUpdateCoroutine()
    {
        while (true)
        {
            _turretRotate.Rotate(MouseManager.Instance.MouseDir);

            if (!TutorialManager.Instance.IsCanMove)
            {
                yield return null;
                continue;
            }

            switch (controlType)
            {
                case ControlType.Detail:
                    DetailControl();
                    break;
                case ControlType.Simple:
                    SimpleControl();
                    break;
            }

            if (!_wasControlled)
            {
                OnPlayerDidNotAnyThing?.Invoke();
            }

            yield return null;
        }
    }

    protected override void ShellAddInput()
    {
        ShellEquipmentData shellEquipmentData = ShellSaveManager.GetShellEquipment(PlayerDataManager.Instance.GetPlayerTankID());
        int shellCnt = 0;

        List<Action> actionList = new List<Action>();
        for (int i = 0; i < shellEquipmentData._shellEquipmentList.Count; i++)
        {
            int dataIndex = i;
            if (shellEquipmentData._shellEquipmentList[dataIndex] == "")
            {
                shellCnt++;
                continue;
            }
            int emptyCnt = shellCnt;
            Shell shell = AddressablesManager.Instance.GetResource<GameObject>(shellEquipmentData._shellEquipmentList[dataIndex]).GetComponent<Shell>();

            _informationCanvas.ShellToggleManager.AddToggle(dataIndex - shellCnt, shell.ID, shell.ShellSprite, (inOn) =>
            {
                if (inOn)
                {
                    _tank.Turret.CurrentShell = shell;
                }
            });

            actionList.Add(() =>
            {
                if (!TutorialManager.Instance.IsCanChangeShell)
                {
                    return;
                }

                int index = dataIndex;
                int cnt = emptyCnt;
                _informationCanvas.ShellToggleManager.TemplateList[index - cnt].isOn = true;
            });
        }
        Action[] actions = actionList.ToArray();
        KeyboardManager.Instance.AddKeyDownActionList(actions);

        if (shellEquipmentData._shellEquipmentList[0] == "")
        {
            _tank.Turret.CurrentShell = AddressablesManager.Instance.GetResource<GameObject>(shellEquipmentData._shellEquipmentList[1]).GetComponent<Shell>();
        }
        else
        {
            _tank.Turret.CurrentShell = AddressablesManager.Instance.GetResource<GameObject>(shellEquipmentData._shellEquipmentList[0]).GetComponent<Shell>();
        }
    }
}
