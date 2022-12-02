using System;

// 임시로 로그인 하려고 사용중인 클래스
namespace Carum.Util
{
    [Serializable]
    public static class RequestDto
    {
        [Serializable]
        public class User
        {
            public string userId;
            public string password;
            public long mainRoomId;

            public User(string userId, string password)
            {
                this.userId = userId;
                this.password = password;
            }
        }

        [Serializable]
        public class Token
        {
            public string accessToken;
            public string refreshToken;

            public Token(string accessToken, string refreshToken)
            {
                this.accessToken = accessToken;
                this.refreshToken = refreshToken;
            }
        }

        [Serializable]
        public class StartDto
        {
            public Token token;
            public int mainRoomId;
            public string petType;
            public string dailyFace;
            public int dailyColor;
            public bool todayDiary;

        }

        [Serializable]
        public class PetConversationDto
        {
            public string text;
            public string emotion;
        }

        [Serializable]
        public class RoomDto
        {
            public int roomId;
        }

        [Serializable]
        public class PetVisualDto
        {
            public string color;
            public string face;
        }
    }
}