<script setup>
  import Sidebar from '../components/Sidebar.vue'
  import Modal from '../components/Modal.vue';
  import { ref } from "vue";
  import axios from "axios";

  const showModal = ref(false);
  const editModal = ref(false);
  const deleteModal = ref(false);

  const cep = ref('');
  const endereco = ref({});


let clienteSelecionado = {}

const clientes = ref([  
  { 
    nome: 'José', 
    email: 'jose@email.com', 
    telefone: '456456456', 
    logradouro: 'Rua E', 
    cep: '54321-987', 
    cidade: 'Porto Alegre', 
    estado: 'RS' 
  },
  { 
    nome: 'Carla', 
    email: 'carla@email.com', 
    telefone: '789789789', 
    logradouro: 'Av. F', 
    cep: '98765-432', 
    cidade: 'Fortaleza', 
    estado: 'CE' 
  },
  { 
    nome: 'Marcos', 
    email: 'marcos@email.com', 
    telefone: '321321321', 
    logradouro: 'Rua G', 
    cep: '65432-109', 
    cidade: 'Recife', 
    estado: 'PE' 
  },
  { 
    nome: 'Lúcia', 
    email: 'lucia@email.com', 
    telefone: '654654654', 
    logradouro: 'Av. H', 
    cep: '12345-678', 
    cidade: 'Curitiba', 
    estado: 'PR' 
  },
  { 
    nome: 'Rafael', 
    doc: '123123',
    email: 'rafael@email.com', 
    telefone: '987987987', 
    logradouro: 'Rua I', 
    cep: '87654-321', 
    cidade: 'Manaus', 
    estado: 'AM' 
  },
  { 
    nome: 'Sandra', 
    doc: '123123',
    email: 'sandra@email.com', 
    telefone: '159159159', 
    logradouro: 'Rua J', 
    cep: '23456-789', 
    cidade: 'Natal', 
    estado: 'RN' 
  },
  { 
    nome: 'Rodrigo', 
    doc: '123123',
    email: 'rodrigo@email.com', 
    telefone: '357357357', 
    logradouro: 'Av. K', 
    cep: '54321-678', 
    cidade: 'Vitória', 
    estado: 'ES' 
  },
  { 
    nome: 'Juliana', 
    doc: '123123',
    email: 'juliana@email.com', 
    telefone: '258258258', 
    logradouro: 'Rua L', 
    cep: '98765-432', 
    cidade: 'Goiânia', 
    estado: 'GO' 
  },
  { 
    nome: 'Fernando', 
    doc: '123123',
    email: 'fernando@email.com', 
    telefone: '654987321', 
    logradouro: 'Av. M', 
    cep: '87654-321', 
    cidade: 'Florianópolis', 
    estado: 'SC' 
  },
  { 
    nome: 'Aline', 
    doc: '123123',
    email: 'aline@email.com', 
    telefone: '321654987', 
    logradouro: 'Rua N', 
    cep: '23456-789', 
    cidade: 'João Pessoa', 
    estado: 'PB' 
  },
  { 
    nome: 'Gabriel', 
    doc: '123123',
    email: 'gabriel@email.com', 
    telefone: '987123654', 
    logradouro: 'Av. O', 
    cep: '54321-987', 
    cidade: 'Campo Grande', 
    estado: 'MS' 
  },
  { 
    nome: 'Patrícia', 
    doc: '123123',
    email: 'patricia@email.com', 
    telefone: '741852963', 
    logradouro: 'Rua P', 
    cep: '87654-321', 
    cidade: 'Teresina', 
    estado: 'PI' 
  },
  { 
    nome: 'Felipe', 
    doc: '123123',
    email: 'felipe@email.com', 
    telefone: '369258147', 
    logradouro: 'Av. Q', 
    cep: '98765-432', 
    cidade: 'Cuiabá', 
    estado: 'MT' 
  },
  { 
    nome: 'Vanessa', 
    doc: '123123',
    email: 'vanessa@email.com', 
    telefone: '123456789', 
    logradouro: 'Rua R', 
    cep: '23456-789', 
    cidade: 'Maceió', 
    estado: 'AL' 
  },
  { 
    nome: 'Hugo', 
    doc: '123123',
    email: 'hugo@email.com', 
    telefone: '987654321', 
    logradouro: 'Av. S', 
    cep: '54321-987', 
    cidade: 'Belém', 
    estado: 'PA' 
  },
  { 
    nome: 'Lucas', 
    doc: '123123',
    email: 'lucas@email.com', 
    telefone: '987654321', 
    logradouro: 'Rua T', 
    cep: '87654-321', 
    cidade: 'Porto Velho', 
    estado: 'RO' 
  },
  { 
    nome: 'Mariana', 
    doc: '123123',
    email: 'mariana@email.com', 
    telefone: '987654321', 
    logradouro: 'Av. U', 
    cep: '54321-987', 
    cidade: 'São Paulo', 
    estado: 'SP' 
  },
  { 
    nome: 'Luiz', 
    doc: '123123',
    email: 'luiz@email.com', 
    telefone: '987654321', 
    logradouro: 'Rua V', 
    cep: '87654-321', 
    cidade: 'Porto Alegre', 
    estado: 'RS' 
  },
  { 
    nome: 'Isabela', 
    doc: '123123',
    email: 'isabela@email.com', 
    telefone: '987654321', 
    logradouro: 'Av. W', 
    cep: '54321-987', 
    cidade: 'Salvador', 
    estado: 'BA' 
  },
  { 
    nome: 'Tiago', 
    doc: '123123',
    email: 'tiago@email.com', 
    telefone: '987654321', 
    logradouro: 'Rua X', 
    cep: '87654-321', 
    cidade: 'Brasília', 
    estado: 'DF' 
  },
]);

</script>
<template>
  <div class="flex w-full gap-8"  >
    <Sidebar />
    <div class="p-7  w-full">
      <div class="client__header">
        <div class="flex justify-around items-center">
          <h1 class=" text-xl font-bold text-gray-400">Lista de Rotas</h1>
          <button class="modal__btn" @click="showModal = true">
            <div class="flex items-center">
              <img class="mr-2" src="/src/assets/svg/add-location.svg" alt="">
              <span>Adicionar Rota</span>
            </div>
          </button>
        </div>
      </div>
      <div class="client__body">
        <div class="container w-full mx-auto py-8">
            <img class="block w-full max-w-[720px] mx-auto" src="/src/assets/svg/construction.svg" alt="">
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
  .close__btn {
    position: absolute;
    top: 20px;
    right: 20px;
  }
</style>
