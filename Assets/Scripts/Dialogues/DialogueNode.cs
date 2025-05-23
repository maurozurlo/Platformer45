﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueNode
{
    public int NodeId;
    [TextArea(3,10)]
    public string Text;
    public List<DialogueOption> Options;
}
