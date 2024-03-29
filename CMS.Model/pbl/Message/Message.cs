﻿using CMS.Model;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace CMS.Model
{
    public class Message : BaseModel<Message>
    {
        public MessageType Type { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }
}