using System;

namespace Common.Messaging
{
    /// <summary>
    /// 데이터를 포함하는 메시지 베이스 클래스.
    /// MessagePool에서 Create해서 가져다가 사용한다.
    /// </summary>
    public abstract class Message : IMessage, IDisposable
    {
        /// <summary>
        /// Release 시 호출. 리셋할 데이터가 있는 메시지만 오버라이드.
        /// </summary>
        public virtual void Dispose()
        {
        }
    }
}
