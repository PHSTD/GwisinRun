public class MonsterDoorOpen : IMonsterState 
{
    private Monster monster;
    //몬스터 이동 경로에 장애물을 체크
    //문일경우에 
    public void SetMonster(Monster monster)
    {
        this.monster = monster;
    }
    public void OnEnter()
    {
        // monster.StartCustomCoroutine();
    }

    public void OnExit()
    {
        monster.StopCustomCoroutine();
    }

    public void OnUpdate()
    {
    }
}