namespace Project.Scripts.Constants
{
    public class MessageConstants
    {
        public static string MessageNotUndo = "Ôi, không có bước đi nào để hoàn tác";
        public static string MessageUnlock(int level)
       {
           return $"Tính năng mới sẽ được mở khóa tại cấp độ {level}."; 
       }  
       
    }
}
