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

  const buscarEndereco = async () => {
    const inputs = document.querySelectorAll('.input__adress')
    console.log(inputs)
  try {
    const response = await axios.get(`https://api.postmon.com.br/v1/cep/${cep.value}`);
    
    if (response.data && Object.keys(response.data).length !== 0) {
      endereco.value = response.data;
    } else {
      inputs.forEach(element => { 
        element.value = "";
      });
      console.error('Endereço não encontrado para o CEP fornecido.');
    }
    
    console.log(endereco.value);
  } catch (error) {
    console.error('Erro ao buscar endereço:', error);
  }
};

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


  const editarCliente = (index) => {
    console.log("Editar cliente:", clientes.value[index]);

    clienteSelecionado = clientes.value[index]
  };

  const apagarCliente = (index) => {
    clienteSelecionado = clientes.value[index]

    console.log("Apagar cliente:", clientes.value[index]);
  };

  const deleteCliente = (index) => {
    console.log(index)
    clientes.value.splice(index, 1); 
  }


</script>
<template>
  <div class="flex gap-8"  >
    <Sidebar />
    <div class="p-7  w-full">
      <div class="client__header">
        <div class="flex justify-around items-center">
          <h1 class=" text-xl font-bold text-gray-400">Lista de clientes</h1>
          <button class="modal__btn" @click="showModal = true">
            <div class="flex items-center">
              <img class="mr-2" src="/src/assets/svg/add-profile.svg" alt="">
              <span>Adicionar Cliente</span>
            </div>
          </button>
        </div>
      </div>
      <div class="client__body">
        <div class="container mx-auto py-8">
          <table class="w-full border-collapse border border-gray-200 rounded">
            <thead>
              <tr class="bg-gray-100">
                <th class="py-2 px-4 border border-gray-200">Nome</th>
                <th class="py-2 px-4 border border-gray-200">Email</th>
                <th class="py-2 px-4 border border-gray-200">Telefone</th>
                <th class="py-2 px-4 border border-gray-200">Cidade/Estado  </th>
                <th class="py-2 px-4 border border-gray-200">Ações</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(cliente, index) in clientes" :key="index" class="bg-white">
                <td class="py-2 px-4 border border-gray-200">{{ cliente.nome }}</td>
                <td class="py-2 px-4 border border-gray-200">{{ cliente.email }}</td>
                <td class="py-2 px-4 border border-gray-200">{{ cliente.telefone }}</td>
                <td class="py-2 px-4 border border-gray-200">{{ cliente.cidade }}/{{ cliente.estado }}</td>
                <td class="py-2 px-4 border border-gray-200">
                  <div class="flex justify-center">
                    <button @click="editarCliente(index), editModal = true" class="mr-3">
                      <img src="/src/assets/svg/edit.svg" alt="Editar" title="Editar">
                    </button>
                    <button @click="apagarCliente(index), deleteModal = true" class="">
                      <img src="/src/assets/svg/trash.svg" alt="Apagar" title="Apagar">
                    </button>
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
  <Modal :show="showModal">
    <h2 class="text-center text-lg mb-8">Cadastrar Cliente</h2>
    <div class="grid grid-cols-2 gap-4">
      <div class="input__box mb-2">
        <label for="nome" class="block w-full">Nome:</label>
        <input class="w-full h-[40px] p-2 border rounded mt-2" name="nome" type="text" placeholder="Digite o nome">
      </div>
      <div class="input__box mb-2">
        <label for="doc" class="block w-full">CPF/CNPJ:</label>
        <input class="w-full h-[40px] p-2 border rounded mt-2" name="doc" type="text" placeholder="Digite o CPF/CNPJ">
      </div>
      <div class="input__box">
        <label for="doc" class="block w-full">Telefone:</label>
        <input class="w-full h-[40px] p-2 border rounded mt-2" name="doc" type="text" placeholder="Digite o telefone">
      </div>
      <div class="input__box">
        <label for="email" class="block w-full">E-mail:</label>
        <input class="w-full h-[40px] p-2 border rounded mt-2" name="email" type="text" placeholder="Digite o E-mail">
      </div>
      <div class="input__box">
        <label for="cep" class="block w-full">CEP:</label>
        <input class="w-full h-[40px] p-2 border rounded mt-2" name="cep" type="text" v-model="cep" @input="buscarEndereco" placeholder="Digite o CEP">
      </div>
      <div class="input__box">
        <label for="logradouro" class="block w-full">Logradouro:</label>
        <input class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="logradouro" type="text" placeholder="Digite o logradouro" v-model="endereco.logradouro" disabled>
      </div>
      <div class="input__box">
        <label for="bairro" class="block w-full">Bairro:</label>
        <input class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="bairro" type="text" placeholder="Digite o bairro" v-model="endereco.bairro" disabled>
      </div>
      <div class="input__box">
        <label for="estado" class="block w-full">Estado:</label>
        <input class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="estado" type="text" placeholder="Digite o logradouro" v-model="endereco.estado" disabled>
      </div>
      <div class="input__box">
        <label for="numero" class="block w-full">Número:</label>
        <input class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="numero" type="text" placeholder="Digite o número">
      </div>
      <div class="input__box">
        <label for="complemento" class="block w-full">Complemento:</label>
        <input class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="complemento" type="text" placeholder="Digite o complemento">
      </div>
    </div>
    <button class="close__btn" @click="showModal = false">
      <svg width="30px" height="30px" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg" fill="none"><path stroke="#000000" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 12 7 7m5 5 5 5m-5-5 5-5m-5 5-5 5"/></svg>
    </button>
    <button class="w-full h-[40px] bg-[#b91c1c] rounded mt-4 text-white">
      Cadastrar
    </button>
  </Modal>
  <Modal :show="editModal">
    <h2 class="text-center text-lg mb-8">Editar Cliente</h2>
    <div class="grid grid-cols-2 gap-4">
      <div class="input__box mb-2">
        <label for="nome" class="block w-full">Nome:</label>
        <input class="w-full h-[40px] p-2 border rounded mt-2" name="nome" type="text" v-model="clienteSelecionado.nome" placeholder="Digite o nome">
      </div>
      <div class="input__box mb-2">
        <label for="doc" class="block w-full">CPF/CNPJ:</label>
        <input class="w-full h-[40px] p-2 border rounded mt-2" name="doc" type="text" v-model="clienteSelecionado.doc" placeholder="Digite o CPF/CNPJ">
      </div>
      <div class="input__box">
        <label for="doc" class="block w-full">Telefone:</label>
        <input class="w-full h-[40px] p-2 border rounded mt-2" name="doc" type="text" v-model="clienteSelecionado.telefone" placeholder="Digite o telefone">
      </div>
      <div class="input__box">
        <label for="email" class="block w-full">E-mail:</label>
        <input class="w-full h-[40px] p-2 border rounded mt-2" name="email" type="text" v-model="clienteSelecionado.email" placeholder="Digite o E-mail">
      </div>
      <div class="input__box">
        <label for="cep" class="block w-full">CEP:</label>
        <input class="w-full h-[40px] p-2 border rounded mt-2" name="cep" type="text" v-model="clienteSelecionado.cep" placeholder="Digite o CEP">
      </div>
      <div class="input__box">
        <label for="logradouro" class="block w-full">Logradouro:</label>
        <input class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="logradouro" v-model="clienteSelecionado.logradouro" type="text" placeholder="Digite o logradouro">
      </div>
      <div class="input__box">
        <label for="bairro" class="block w-full">Bairro:</label>
        <input class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="bairro" type="text" v-model="clienteSelecionado.bairro" placeholder="Digite o bairro">
      </div>
      <div class="input__box">
        <label for="estado" class="block w-full">Estado:</label>
        <input class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="estado" type="text" v-model="clienteSelecionado.estado" placeholder="Digite o estado">
      </div>
      <div class="input__box">
        <label for="numero" class="block w-full">Número:</label>
        <input class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="numero" type="text" v-model="clienteSelecionado.numero" placeholder="Digite o número">
      </div>
      <div class="input__box">
        <label for="complemento" class="block w-full">Complemento:</label>
        <input class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="complemento" type="text" v-model="clienteSelecionado.complemento" placeholder="Digite o complemento">
      </div>
    </div>
    <button class="close__btn" @click="editModal = false">
      <svg width="30px" height="30px" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg" fill="none"><path stroke="#000000" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 12 7 7m5 5 5 5m-5-5 5-5m-5 5-5 5"/></svg>
    </button>
    <button class="w-full h-[40px] bg-[#006400] rounded mt-4 text-white">
      Editar
    </button>
  </Modal>
  <Modal :show="deleteModal">
    <h2 class="text-center text-lg mb-8">Deletar {{ clienteSelecionado.nome }}</h2>
    <p class="text-center">Tem certeza que deseja deletar o cliente: {{ clienteSelecionado.nome }} ?</p>
    <button class="close__btn" @click="deleteModal = false">
      <svg width="30px" height="30px" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg" fill="none"><path stroke="#000000" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 12 7 7m5 5 5 5m-5-5 5-5m-5 5-5 5"/></svg>
    </button>
    <button class="w-full h-[40px] bg-[#b91c1c] rounded mt-4 text-white" @click="deleteCliente(clienteSelecionado), deleteModal = false">
      Deletar
    </button>
  </Modal>
</template>

<style scoped>
  .close__btn {
    position: absolute;
    top: 20px;
    right: 20px;
  }
</style>
