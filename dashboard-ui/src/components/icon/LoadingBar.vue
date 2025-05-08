<template>
    <div :style="{ width: width, height: height, background: '#0e96e7', position: 'relative', overflow: 'hidden' }">
      <div class="loading-bar" :style="{ animationDuration: animationDuration }"></div>
    </div>
  </template>
  
  <script lang="ts">
  export default {
    name: 'LoadingBar',
    props: {
      width: {
        type: String,
        default: '100%'
      },
      height: {
        type: String,
        default: '4px'
      },
      animationDuration: {
        type: String,
        default: '4s'
      }
    }
  }
  </script>
  
  <style scoped>
  .loading-bar {
    position: absolute;
    height: 4px;
    width: 80px;
    background: linear-gradient(to right, transparent, #fff);
    animation: left_to_right 4s 0s infinite;
  }
  
  @keyframes left_to_right {
    0% {
      left: -80px;
    }
    100% {
      left: 100%;
    }
  }
  </style>
  