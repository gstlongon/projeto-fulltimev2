<template>
  <div class="modal" v-if="show">
    <div class="modal-content w-full max-w-[500px]">
      <slot></slot>
    </div>
  </div>
</template>

<script>
export default {
  name: 'Modal2',
  props: {
    show: {
      type: Boolean,
      required: true
    }
  }
}
</script>

<style scoped>
.modal {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
}

.modal-content {

  background-color: white;
  padding: 20px;
  border-radius: 5px;
  position: relative;
}

</style>





