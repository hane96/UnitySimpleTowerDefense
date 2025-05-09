# UnitySimpleTowerDefense

這是我從0開始自學unity邊學邊製作的小型練習作品和學習筆記，主要目的是學習unity的基本操作和練習遊戲功能的實作

尚未製作完整的UI介面或進行遊戲平衡的設計


## 專案目標

- 練習 Unity 基礎物件組成和 Prefeb 使用
- 用 C# 控制遊戲邏輯
- 熟悉基本 script 互動、碰撞處理、遊戲流程管理


## 遊戲玩法

- 按 `1` 或 `2` 切換要放置的塔種
- 滑鼠左鍵點擊地圖放置塔
- 塔會自動攻擊範圍內的敵人
- 敵人會沿路徑前進，靠近主堡造成傷害
- 擊殺敵人可以獲得金錢，金錢可以用來放置更多的塔
- 隨時間敵人會變得更快、血量更高
- 主堡血量歸0會遊戲結束


## 專案結構簡介

- `Assets/`：包含script、Prefab
- `Packages/`：Unity 套件管理資料夾
- `ProjectSettings/`：設定檔

---

## 主要物件功能說明

### MoneyManager
- 管理玩家金錢狀態（初始金額、加錢、扣錢）
- 顯示金錢 UI

### TowerManager
- 鍵盤選擇塔類型（1、2）
- 滑鼠點擊放塔，消耗金錢
- 控制塔 Prefab 的選擇與生成

### EnemySpawner
- 固定時間生成敵人
- 記錄生成次數，每隔幾次生成就讓敵人血量和速度提升

### EnemyMovement
- 控制敵人沿指定路徑前進
- 血量歸0或到達castle時死亡
- 承受傷害、對castle造成傷害、死亡時掉落金錢

### Castle/CastleUI
- 儲存並顯示血量
- 血量歸0時顯示遊戲結束

### TowerType1
- 自動檢測範圍內敵人並造成範圍傷害

### TowerType2
- 自動產生子彈鎖定敵人攻擊
- 決定子彈的方向、射程、傷害等

### bullet
- 碰撞時對敵人造成傷害
- 超過射程時自動摧毀

---



## 後續可能改進方向

- 加入更多種類的塔（範圍攻擊、減速塔等）
- 完善 UI 顯示（塔血量、敵人波次等）
- 增加關卡系統或升級系統
- 製作完整 Build 版本（ex: WebGL / exe）

---


更詳細的學習筆記與製作過程請參考：[note.md](note.md)

