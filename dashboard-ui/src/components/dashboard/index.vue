<template>
    <div class="container">
        <dv-full-screen-container>

            <div class="header">
                <img src="@/assets/国创电力.png" alt="Logo" class="logo" />
                <div class="center-title">蝶阀自动装配线电子看板</div>
                <TimeDisplay class="time-display"/>
            </div>
            <div class="main">

                <div class="main-top">
                    <dv-border-box-1>
                        <dv-border-box-8 :dur="9999999999">
                            <deivceStatus />
                        </dv-border-box-8>
                    </dv-border-box-1>
                    
                </div>
                <div class="main-bottom">
                    <div class="bottom-left">
                        <dv-border-box-1>
                            <dv-border-box-8 :dur="9999999999">
                                <capacity />
                            </dv-border-box-8>
                        </dv-border-box-1>
                    </div>
                    <div class="bottom-middle">
                        <dv-border-box-1>
                            <dv-border-box-8 :dur="9999999999">
                                <alarm />
                            </dv-border-box-8>
                        </dv-border-box-1>
                    </div>
                    <div class="bottom-right">
                        <dv-border-box-1>
                            <dv-border-box-8 :dur="9999999999">
                                <process />
                            </dv-border-box-8>
                        </dv-border-box-1>
                    </div>

                </div>
            </div>
        </dv-full-screen-container>
    </div>
</template>
<script  setup lang="ts">
import TimeDisplay from '../icon/TimeDisplay.vue';
import deivceStatus from "../deviceStatus/index.vue";
import capacity from "../capacity/index.vue";
import alarm from "../alarmInformation/index.vue";
import process from "../processparameter/index.vue";

</script>
<style lang="scss" scoped>
.container {
    width: 100%;
    height: 100%;

    color: #fff;
    background-image: url("@/assets/图层5.png");
    background-size: 100% 100%;

    .header {

        height: 8%;
        display: flex;
        justify-content: space-between;
        align-items: center; /* 垂直居中对齐 */

        .logo {
            position: absolute;
            left: 20px;
            top:10px;
            width: 100px; /* 根据实际需求调整大小 */
            height: auto;
        }

        .header-center-decoration {
            width: 40%;
            height: 100%;
            margin-top: 30px;
        }

        .header-left-decoration {
            width: 25%;
            height: 100%;
        }

        .header-right-decoration {
            width: 25%;
            height: 100%;
        }

        .center-title {
            position: absolute;
            font-size: 30px;
            font-weight: bold;
            left: 50%;
            top: 15px;
            transform: translateX(-50%);
        }
    }

    .main {
        height: calc(100% - 80px);
        display: flex;
        flex-direction: column;

        .main-top {
            margin-top: 2%;
            width: 100%;
            height: 40%;

        }

        .main-bottom {
            width: 100%;
            height: 50%;
            display: flex;
            justify-content: space-between;

            .bottom-left {
                width: 25%;
            }

            .bottom-middle{
                width: 10%;
            }

            .bottom-right {
                width: 65%;
            }
        }
    }
}
.time-display {
  margin-left: auto; /* 推动 TimeDisplay 组件到右侧 */
  right: 50px;
}
</style>




