<template>
  <div>
    <p>{{ time }}</p>
  </div>
</template>

<script>
export default {
  data() {
    return {
      time: this.formatTime(new Date())
    };
  },
  mounted() {
    this.startClock();
  },
  methods: {
    formatTime(date) {
      const year = date.getFullYear();
      const month = String(date.getMonth() + 1).padStart(2, '0'); // 月份从0开始，需要加1
      const day = String(date.getDate()).padStart(2, '0');
      const hours = String(date.getHours()).padStart(2, '0');
      const minutes = String(date.getMinutes()).padStart(2, '0');
      const seconds = String(date.getSeconds()).padStart(2, '0');
      return `${year}-${month}-${day} ${hours}:${minutes}:${seconds}`;
    },
    startClock() {
      this.clockInterval = setInterval(() => {
        this.time = this.formatTime(new Date());
      }, 1000);
    }
  },
  beforeDestroy() {
    clearInterval(this.clockInterval);
  }
};
</script>

<style scoped>
p {
  font-size: 2em;
  font-weight: bold;
}
</style>
