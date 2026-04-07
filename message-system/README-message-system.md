Message System

캐리캐리에서 사용하는 구독 / 발행 기반 메시지 시스템입니다.
큐 기반으로 구독 -> 해제 -> 발행 순서를 유지하며, Message 객체는 풀링으로 재사용합니다.

- 특징

- 큐 기반 Flush
LateUpdate에서 일괄 처리하여 메시지 처리 순서를 유지합니다.

- SubscribeOnce
1회성 구독을 지원합니다.

- MessagePool
제네릭 풀링을 사용하여 Message 인스턴스를 재사용합니다.

- 파일 구성

- IMessage.cs : 메시지 공통 인터페이스
- Message.cs : 데이터 포함 메시지 베이스 (풀링 대상)
- MessageSystem.cs : Subscribe / Publish / Flush 처리
- MessageSystemRunner.cs : LateUpdate에서 Flush 호출 및 정리
- MessagePool.cs : 제네릭 풀링 구현