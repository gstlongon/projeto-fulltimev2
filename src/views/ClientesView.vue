<script setup>
  import Sidebar from '../components/Sidebar.vue'
  import Swal from 'sweetalert2';
  import Modal from '../components/Modal.vue';
  import { ref, onMounted } from "vue";
  import axios from "axios";

  const showModal = ref(false);
  const editModal = ref(false);
  const deleteModal = ref(false);
  let count = 0

  const cep = ref('');
  let clientes = ref('')

  const novoCliente = ref({
      nome: '',
      cpf: '',
      telefone: '',
      email: '',
      cep: '',
      logradouro: '',
      bairro: '',
      estado: '',
      numero: '',
      complemento: ''
    });

    let clienteSelecionado = ref({
      nome: '',
      email: '',
      cpf: '',
      telefone: '',
      logradouro: '',
      bairro: '',
      cep: '',
      numero: '',
      cidade: '',
      estado: ''
    });

    const endereco = ref({
      logradouro: '',
      bairro: '',
      estado: ''
    });

  const buscarEndereco = async () => {
    const inputs = document.querySelectorAll('.input__adress')
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

  const buscarClientes = async () => {
  try {
    const response = await axios.get(`https://localhost:7204/api/Cliente`);
    
    if (response.data && Object.keys(response.data).length !== 0) {
        clientes.value = response.data
    } 
  } catch (error) {
    console.error('Erro ao pegar clientes: ', error);
  }
  };

  const dataForm = async (index) => {
    clienteSelecionado = {
      id: clientes.value[index].id,
      nome: clientes.value[index].nome,
      cpf: clientes.value[index].cpf,
      telefone: clientes.value[index].telefone,
      email: clientes.value[index].email,
      cep: clientes.value[index].cep,
      logradouro: clientes.value[index].logradouro,
      bairro: clientes.value[index].bairro,
      estado: clientes.value[index].estado,
      numero: clientes.value[index].numero,
      cidade: clientes.value[index].cidade
    }
    console.log(clienteSelecionado)
  }

  const adicionarCliente = async () => {  
      try {
        const response = await axios.post('https://localhost:7204/api/Cliente', {
          nome: novoCliente.value.nome,
          email: novoCliente.value.email,
          telefone: novoCliente.value.telefone,
          cpf: novoCliente.value.cpf,
          logradouro: endereco.value.logradouro,
          bairro: endereco.value.bairro,
          cep: cep.value,
          numero: novoCliente.value.numero,
          cidade: endereco.value.cidade,
          estado: endereco.value.estado,
        });
        
        if (response.status === 201) { 
          Swal.fire({
            icon: 'success',
            title: 'Cliente cadastrado com sucesso!'
          });
          novoCliente.value = {
            nome: '',
            cpf: '',
            telefone: '',
            email: '',
            cep: '',
            logradouro: '',
            bairro: '',
            estado: '',
            numero: '',
            complemento: ''
          };
          endereco.value = {
            logradouro: '',
            bairro: '',
            estado: ''
          };
          cep.value = ""
          showModal.value = false; 
          buscarClientes()
          count++
          console.log('contador: ', count)
        }
      } catch (error) {
        console.error('Erro ao adicionar cliente: ', error);
      }
  };

  const editarCliente = async () => {
    try {
        const response = await axios.put(`https://localhost:7204/api/Cliente/${clienteSelecionado.id}`, {
          nome: clienteSelecionado.nome,
          email: clienteSelecionado.email,
          cpf: clienteSelecionado.cpf,
          telefone: clienteSelecionado.telefone,
          logradouro: clienteSelecionado.logradouro,
          bairro: clienteSelecionado.bairro,
          cep: clienteSelecionado.cep,
          numero: clienteSelecionado.numero,
          cidade: clienteSelecionado.cidade,
          estado: clienteSelecionado.estado,
        });
        
        if (response.status === 201 || response.status === 200 || response.status === 204) { 
          Swal.fire({
            icon: 'success',
            title: 'Cliente editado com sucesso!'
          });
          editModal.value = false; 
          buscarClientes()
        }
      } catch (error) {
        console.error('Erro ao editar cliente: ', error);
      }
  };

  const apagarCliente = async () => {
    try {
      const response = await axios.delete(`https://localhost:7204/api/Cliente/${clienteSelecionado.id}`)
        
        if (response.status === 201 || response.status === 200 || response.status === 204) { 
          Swal.fire({
            icon: 'success',
            title: 'Cliente apagado com sucesso!'
          });
          buscarClientes()  
          editModal.value = false; 
          console.log("numero de clientes: ", clientes.length    ,clientes)
          count--
          if (count < 1) {
            window.location.reload()
          }
        }
      } catch (error) {
        console.error('Erro ao deletar cliente: ', error);
      }
  };

  onMounted(() => {
      buscarClientes();
    });

    console.log('contador: ', count)

</script>
<template>
  <div class="flex w-full gap-8"  >
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
          <div class="" v-if="clientes">
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
                      <button @click="editModal = true, dataForm(index)" class="mr-3">
                        <img src="/src/assets/svg/edit.svg" alt="Editar" title="Editar">
                      </button>
                      <button @click="dataForm(index), deleteModal = true" class="">
                        <img src="/src/assets/svg/trash.svg" alt="Apagar" title="Apagar">
                      </button>
                    </div>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
          <div class="" v-else>
            <div class="p-8 text-center">
              <h2 class="text-xl  font-bold">Nada para mostrar aqui :(</h2>
              <img class="mx-auto mt-8" src="/src/assets/svg/nothing.svg" alt="">
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  <Modal :show="showModal">
    <h2 class="text-center text-lg mb-8">Cadastrar Cliente</h2>
    <div class="grid grid-cols-2 gap-4">
      <div class="input__box mb-2">
        <label for="nome" class="block w-full">Nome:</label>
        <input v-model="novoCliente.nome" class="w-full h-[40px] p-2 border rounded mt-2" name="nome" type="text" placeholder="Digite o nome">
      </div>
      <div class="input__box mb-2">
        <label for="doc" class="block w-full">CPF/CNPJ:</label>
        <input v-model="novoCliente.cpf" class="w-full h-[40px] p-2 border rounded mt-2" name="doc" type="text" placeholder="Digite o CPF/CNPJ">
      </div>
      <div class="input__box">
        <label for="telefone" class="block w-full">Telefone:</label>
        <input v-model="novoCliente.telefone" class="w-full h-[40px] p-2 border rounded mt-2" name="telefone" type="text" placeholder="Digite o telefone">
      </div>
      <div class="input__box">
        <label for="email" class="block w-full">E-mail:</label>
        <input v-model="novoCliente.email" class="w-full h-[40px] p-2 border rounded mt-2" name="email" type="text" placeholder="Digite o E-mail">
      </div>
      <div class="input__box">
        <label for="cep" class="block w-full">CEP:</label>
        <input v-model="cep" @input="buscarEndereco" class="w-full h-[40px] p-2 border rounded mt-2" name="cep" type="text" placeholder="Digite o CEP">
      </div>
      <div class="input__box">
        <label for="logradouro" class="block w-full">Logradouro:</label>
        <input v-model="endereco.logradouro" class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="logradouro" type="text" placeholder="Digite o logradouro" disabled>
      </div>
      <div class="input__box">
        <label for="bairro" class="block w-full">Bairro:</label>
        <input v-model="endereco.bairro" class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="bairro" type="text" placeholder="Digite o bairro" disabled>
      </div>
      <div class="input__box">
        <label for="complemento" class="block w-full">Cidade:</label>
        <input v-model="endereco.cidade" class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="complemento" type="text" placeholder="Digite a cidade" disabled>
      </div>
      <div class="input__box">
        <label for="estado" class="block w-full">Estado:</label>
        <input v-model="endereco.estado" class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="estado" type="text" placeholder="Digite o logradouro" disabled>
      </div>
      <div class="input__box">
        <label for="numero" class="block w-full">Número:</label>
        <input v-model="novoCliente.numero" class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="numero" type="text" placeholder="Digite o número">
      </div>
      
    </div>
    <button class="close__btn" @click="showModal = false">
      <svg width="30px" height="30px" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg" fill="none"><path stroke="#000000" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 12 7 7m5 5 5 5m-5-5 5-5m-5 5-5 5"/></svg>
    </button>
    <button @click="adicionarCliente" class="w-full h-[40px] bg-[#b91c1c] rounded mt-4 text-white">
      Cadastrar
    </button>
  </Modal>
  
  <Modal :show="editModal">
    <h2 class="text-center text-lg mb-8">Editar {{ clienteSelecionado.nome }}</h2>
    <div class="grid grid-cols-2 gap-4">
      <div class="input__box mb-2">
        <label for="nome" class="block w-full">Nome:</label>
        <input class="w-full h-[40px] p-2 border rounded mt-2" name="nome" type="text" v-model="clienteSelecionado.nome" placeholder="Digite o nome">
      </div>
      <div class="input__box mb-2">
        <label for="doc" class="block w-full">CPF/CNPJ:</label>
        <input class="w-full h-[40px] p-2 border rounded mt-2" name="doc" type="text" v-model="clienteSelecionado.cpf" placeholder="Digite o CPF/CNPJ">
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
        <label for="complemento" class="block w-full">Cidade:</label>
        <input class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="complemento" type="text" v-model="clienteSelecionado.cidade" placeholder="Digite o complemento">
      </div>
      <div class="input__box">
        <label for="estado" class="block w-full">Estado:</label>
        <input class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="estado" type="text" v-model="clienteSelecionado.estado" placeholder="Digite o estado">
      </div>
      <div class="input__box">
        <label for="numero" class="block w-full">Número:</label>
        <input class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="numero" type="text" v-model="clienteSelecionado.numero" placeholder="Digite o número">
      </div>
      
    </div>
    <button class="close__btn" @click="editModal = false">
      <svg width="30px" height="30px" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg" fill="none"><path stroke="#000000" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 12 7 7m5 5 5 5m-5-5 5-5m-5 5-5 5"/></svg>
    </button>
    <button class="w-full h-[40px] bg-[#006400] rounded mt-4 text-white" @click="editarCliente()">
      Editar
    </button>
  </Modal>
  <Modal :show="deleteModal">
    <h2 class="text-center text-lg mb-8">Deletar {{ clienteSelecionado.nome }}</h2>
    <p class="text-center">Tem certeza que deseja deletar o cliente: {{ clienteSelecionado.nome }} ?</p>
    <button class="close__btn" @click="deleteModal = false">
      <svg width="30px" height="30px" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg" fill="none"><path stroke="#000000" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 12 7 7m5 5 5 5m-5-5 5-5m-5 5-5 5"/></svg>
    </button>
    <button class="w-full h-[40px] bg-[#b91c1c] rounded mt-4 text-white" @click="apagarCliente(), deleteModal = false">
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
