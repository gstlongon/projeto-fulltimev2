<template>
  <div class="modal" v-if="show">
    <div class="modal-content w-full max-w-[500px]">
      <slot></slot>
    </div>
  </div>
</template>

<script>
export default {
  name: 'Modal',
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
  overflow: auto; 
}

.modal-content {
  background-color: white;
  padding: 32px;
  border-radius: 10px; 
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.5); 
  max-width: 50%; 
  max-height: 80%; 
  overflow-y: auto; 
  position: relative;
}

</style>


