# 쿠팡몽

## 0. 프로젝트 소개
이 프로젝트는 스테이지 클리어를 목표로 한 액션 퍼즐 장르의 게임이다.
핵심 플레이는 테트리스와 유사하며, 플레이어는 각종 방해요소들을 피해 기믹을 수행하게 된다.

- 프로젝트 이름 : 쿠팡몽
- 장르 : 퍼즐 액션 (3D 테트리스)
- 플레이타임 : 1시간
- 개발 환경 (유니티 버전) : Unity 2022.3.62f2
- 플랫폼: Windows
- 프로젝트 기간 : 7일
- 팀장 : 이효석
- 팀원 : 강세환, 안정연, 조민형, 정재훈
- 시연 영상 : https://www.youtube.com/watch?v=2ogE45Vr1hc

## 1. 게임 특징

### 1-1. 핵심 메커니즘

#### [블록]
- 블록을 상호작용해 가로 및 세로 1줄이상을 만든다
- 클리어한 라인의 배수로 점수 획득 가능 (3줄 클리어시 900점 획득)
- 라인을 클리어 한 뒤 남은 블록은 상호작용 불가
- 스테이지 목표 점수에 도달해 다음 스테이지로 넘어간다

#### [방해 블록]
- 일정 시간마다 스테이지 별로 정해진 개수의 방해블록 드랍
- 상호작용 불가능
- 라인클리어에는 기여함

#### [몬스터]
- 두가지 종류의 몬스터가 정해진 시간마다 랜덤으로 생성
- [Patrol]
  - 생성된 후 일정 거리를 순찰
  - 블록에 맞으면 사망
  - 사망할 때 정해진 거리내에 플레이어가 있으면 플레이어에게 STUN 디버프 부여
  - STUN : 일정 시간 Player는 움직일 수 없음

- [Scout]
  - 생성된 후 플레이어를 쫒아감
  - 블록에 맞으면 사망
  - 플레이어에게 부딪히면 사망처리되며 플레이어에게 SLOW 디버프 부여
  - SLOW : 일정 시간 Player의 이동속도 감소

### 1-2. 프로그램 구조

```
                    [ GameScene ]
                          │
                          ▼
                   [ GameManager ]
                          │
        ┌─────────────────┼─────────────────┐
        ▼                 ▼                 ▼
 [ StageSystem ]   [ ScoreSystem ]     [ GameEventBus ]
                                                │
                         ┌──────────────────────┼──────────────────────┐
                         ▼                      ▼                      ▼
                      [ Block ]             [ Monster ]             [ Player ]
                         │                      │                      │
                         └──────────────┐       │       ┌──────────────┘
                                        ▼       ▼
                                     [ Grid ]  (Collision / Line Clear)

────────────────────────────────────────────────────────────────────────

                    [ PoolManager (Singleton) ]
                              │
              ┌───────────────┼───────────────┐
              ▼                               ▼
        [ Block Pool ]                  [ Monster Pool ]
```

#### [Object Pooling (Singleton Pattern)]
- 잦은 오브젝트 생성/삭제로 인한 성능 저하를 방지하기 위해
Singleton 기반 Object Pooling 구조를 사용한다.

- PoolManager
  - 풀 전체를 관리하는 싱글톤 매니저
  - 각 타입별 ObjectPool을 생성 및 관리

- ObjectPool
  - 오브젝트 초기화 및 재사용 관리
  - 주요 기능
    - Initialize() : 풀 초기화
    - Spawn() : 오브젝트 활성화
    - Despawn() : 오브젝트 비활성화
    - ExpandPool() : 풀 확장

- IPoolable
  - 풀링 대상 오브젝트 인터페이스
  - OnDisableObject()를 통해 공통 비활성화 처리

- 블록, 몬스터, 이펙트 등 반복 생성되는 모든 오브젝트에 적용하여
런타임 성능을 안정적으로 유지한다.

#### [Block]
- 블록의 생성, 회전, 이동, 착지 등 테트리스 핵심 로직을 담당하는 구조이다.

- BlockController
  - 블록 전체 흐름 제어
  - 일반 블록 및 고스트 블록 생성

- BlockFactory
  - 블록 타입에 따라 블록 생성 책임 분리

- BlockShape
  - 블록의 회전 및 착지 처리
  - RotateLeft(), RotateRight(), Land()

- BlockState
  - 블록 상태 관리 (Falling, Landed 등)

- BlockView
  - 블록의 시각적 표현 담당

- GhostBlock
  - 착지 예상 위치를 표시하는 보조 블록

- 블록 로직과 표현을 분리하여 유지보수 및 확장성을 고려하였다.

#### [Monster]

- MonsterSpawner
  - 일정 시간마다 몬스터 생성

- MonsterFactory
  - 몬스터 타입에 따라 생성 로직 분리

- MonsterController
  - 몬스터 상태 및 행동 제어

- MonsterMovement
  - 이동 로직 관리

- MonsterAttack
  - 공격 및 피격 처리

- PatrolMonster
  - 일정 구간을 순찰하는 몬스터

- ScoutMonster
  - 플레이어를 추적하는 몬스터

- 모든 몬스터는 Object Pooling 구조를 사용하여 성능을 최적화하였다.

#### [GameEventBus (Observer Pattern)]
- 게임 내 시스템 간 의존성을 줄이기 위해
Observer 패턴 기반 이벤트 시스템을 사용한다.

- GameEventBus
  - 이벤트 등록 / 해제 / 발행 관리
  - Subscribe(), Unsubscribe(), Raise()

- 주요 이벤트
  - StageStartedEvent, StageClearedEvent
  - LineClearedEvent
  - ScoreUpdatedEvent
  - HeldEvent
  - SceneLoadedEvent

- 각 시스템은 이벤트를 통해 간접적으로 통신하여
결합도를 낮추고 구조를 단순화하였다.

#### [Grid]
- 블록 배치 및 라인 클리어 판정을 담당하는 구조이다.

- GridTile
  - 개별 그리드 셀 관리

- LineChecker
  - 가로 / 세로 라인 완성 여부 검사

- BlockBuster
  - 라인 클리어 시 블록 제거 처리

- CanBlock, CanTBlock, CanZBlock 등
  - 블록 배치 가능 여부 판단 로직

- 라인 클리어 이후 남은 블록은 상호작용 불가 상태로 전환된다.

#### [Player]

- 플레이어 이동 및 상태이상 처리를 담당한다.

- PlayerController
  - 플레이어 전체 상태 관리

- PlayerMovement
  - 이동 처리

- PlayerInteraction
  - 블록 상호작용

- PlayerState
  - 상태 관리 (Normal, Stun, Slow)

- PlayerAnimator
  - 애니메이션 제어

- 몬스터와의 충돌 및 디버프 효과(STUN, SLOW)를 상태 기반으로 처리한다.

#### [GameManager / StageManager / ScoreManager -> GameScene]

- 게임 전체 흐름을 관리하는 상위 구조이다.

- GameManager
  - 게임 상태 관리 (Start, Pause, GameOver)

- StageSystem
  - 스테이지 진행 및 목표 점수 관리

- ScoreSystem
  - 점수 계산

- GameScene
  - UI 갱신

- 각 시스템을 통합하여 실제 플레이 씬 구성

### 1-3. SnapShot

#### [Title]

<img width="1919" height="897" alt="image" src="https://github.com/user-attachments/assets/ef79b2ae-d29b-44ec-82d8-27b2d92e1ada" />

- 게임 시작 시 스테이지 진입

<img width="1919" height="890" alt="image" src="https://github.com/user-attachments/assets/a807ed31-5afc-49de-83bc-b6a096188f5e" />

- 게임 종료 시 팝업창 표출 후 Yes버튼 클릭 시 게임 종료

#### [GameScene]

<img width="1919" height="893" alt="image" src="https://github.com/user-attachments/assets/b14a4e0f-b9c9-43fe-98f6-da201c56f56c" />

- 스테이지 돌입 후 SpaceBar키 입력 시 스테이지 시작

<img width="1919" height="892" alt="image" src="https://github.com/user-attachments/assets/e5c16da7-8f4b-44e9-b4e5-385606611cc3" />

- Player가 상태이상에 걸릴 시 UI 표출

#### [Pause]

<img width="1919" height="893" alt="image" src="https://github.com/user-attachments/assets/369df3d8-dbe4-493e-a2a4-b228e8a211b7" />

- ESC 키 입력 시 Pause 상태 진입
- Pause상태에 진입 시 게임이 멈추고 타이틀로 돌아가기 / 재시작 가능
- 타이틀로 돌아가기 : Title 화면으로 돌아감
- 재시작 : 해당 스테이지를 초기화 하고 재시작
- Esc 재 입력시 Pause 상태 해제

#### [Change Camera point]

<img width="1919" height="880" alt="image" src="https://github.com/user-attachments/assets/c23d2ea2-d341-4fd1-98c4-a1d6b3f45b4d" />

- Enter키 입력 시 화면 전환 가능

#### [GameOver]

<img width="1919" height="894" alt="image" src="https://github.com/user-attachments/assets/62c316bc-6e0f-48a3-b38d-1117d8e8dd52" />

- 플레이어가 블록에 맞으면 사망 이펙트 표출 후 게임 오버씬 출력
- 버튼 기능은 Pause와 동일

#### [GameClear]

<img width="1919" height="893" alt="image" src="https://github.com/user-attachments/assets/d452faf7-b116-4d6c-a20f-dbabd982cf6f" />

- 주어진 스테이지 모두 클리어시 게임 클리어 씬 표출
- 텍스트가 상단으로 점점 올라가며 다 올라간 뒤 타이틀로 버튼 출력

#### [Player]

<img width="1919" height="880" alt="image" src="https://github.com/user-attachments/assets/e72b6ae1-5887-46cc-ac87-a164f244cf21" />

- 떨어지는 블록이나 방해 블록에 맞으면 애니메이션 표출 후 사망

## 2. 플레이 방법
- 이동 : W A S D
- 블록 잡기 / 놓기 : SPACE
- 블록 회전 : Q E
- 블록 이동 : ← → ↑ ↓ 
- 일시 정지 : ESC
- 시점 변경 : ENTER

## 3. 실행 환경
- Unity: 2022.3.62f2
- 플랫폼: Windows

## 4. 협업 관리
- Notion
- Git
- Figma
