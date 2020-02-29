public enum objStatus
{
    NON = -1,
    set = 0,//状態のセッティング
    stay,//待機->等速で移動
    play,//操作または敵ならアクティブ化
    loss,//撃破,HP0->残機減算,ゲームオーバーまたは敵ならデストロイ処理
}

public enum BOSS_Status
{
    NON = -1,
    set = 0,//状態のセッティング
    stay,//待機->等速で移動
    play_1,//アクティブ化,ステップ1
    play_2,//アクティブ化,ステップ2
    play_3,//アクティブ化,ステップ3
    loss,//デストロイ処理
}

public enum shellStatus
{
    NON=-1,
    set=0,
    mode1,//アクティブ化,ステップ1
    mode2,//アクティブ化,ステップ2
    mode3,//アクティブ化,ステップ3
    loss,
}

