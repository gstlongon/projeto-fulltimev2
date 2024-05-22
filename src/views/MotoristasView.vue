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
  let motoristas = ref('')

  const novoMotorista = ref({
      nome: '',
      cpf: '',
      telefone: '',
      email: '',
      tipoVeiculo: '',
      placaVeiculo: '',
      cep: '',
      logradouro: '',
      bairro: '',
      estado: '',
      numero: '',
      complemento: ''
    });

    let motoristaselecionado = ref({
      nome: '',
      email: '',
      tipoVeiculo: '',
      placaVeiculo: '',
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

  const buscarmotoristas = async () => {
  try {
    const response = await axios.get(`https://localhost:7204/api/Motorista`);
    
    if (response.data && Object.keys(response.data).length !== 0) {
        motoristas.value = response.data
    } 
  } catch (error) {
    console.error('Erro ao pegar motorista: ', error);
  }
  };

  const dataForm = async (index) => {
    motoristaselecionado = {
      id: motoristas.value[index].id,
      nome: motoristas.value[index].nome,
      cpf: motoristas.value[index].cpf,
      tipoVeiculo: motoristas.value[index].tipoVeiculo,
      placaVeiculo: motoristas.value[index].placaVeiculo,
      telefone: motoristas.value[index].telefone,
      email: motoristas.value[index].email,
      cep: motoristas.value[index].cep,
      logradouro: motoristas.value[index].logradouro,
      bairro: motoristas.value[index].bairro,
      estado: motoristas.value[index].estado,
      numero: motoristas.value[index].numero,
      cidade: motoristas.value[index].cidade
    }
    console.log(motoristaselecionado)
  } 

  const adicionarMotorista = async () => {  
      try {
        const response = await axios.post('https://localhost:7204/api/Motorista', {
          nome: novoMotorista.value.nome,
          email: novoMotorista.value.email,
          telefone: novoMotorista.value.telefone,
          cpf: novoMotorista.value.cpf,
          tipoVeiculo: novoMotorista.value.tipoVeiculo,
          placaVeiculo: novoMotorista.value.placaVeiculo,
          logradouro: endereco.value.logradouro,
          bairro: endereco.value.bairro,
          cep: cep.value,
          numero: novoMotorista.value.numero,
          cidade: endereco.value.cidade,
          estado: endereco.value.estado,
        });
        
        if (response.status === 201) { 
          Swal.fire({
            icon: 'success',
            title: 'Motorista cadastrado com sucesso!'
          });
          novoMotorista.value = {
            nome: '',
            cpf: '',
            tipoVeiculo:'',
            placaVeiculo:'',
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
          buscarmotoristas()
          count++
          console.log('contador: ', count)
        }
      } catch (error) {
        console.error('Erro ao adicionar motorista: ', error);
      }
  };

  const editarMotorista = async () => {
    try {
        const response = await axios.put(`https://localhost:7204/api/Motorista/${motoristaselecionado.id}`, {
          nome: motoristaselecionado.nome,
          email: motoristaselecionado.email,
          cpf: motoristaselecionado.cpf,
          tipoVeiculo: motoristaselecionado.tipoVeiculo,
          placaVeiculo: motoristaselecionado.placaVeiculo,
          telefone: motoristaselecionado.telefone,
          logradouro: motoristaselecionado.logradouro,
          bairro: motoristaselecionado.bairro,
          cep: motoristaselecionado.cep,
          numero: motoristaselecionado.numero,
          cidade: motoristaselecionado.cidade,
          estado: motoristaselecionado.estado,
        });
        
        if (response.status === 201 || response.status === 200 || response.status === 204) { 
          Swal.fire({
            icon: 'success',
            title: 'Motorista editado com sucesso!'
          });
          editModal.value = false; 
          buscarmotoristas()
        }
      } catch (error) {
        console.error('Erro ao editar motorista: ', error);
      }
  };

  const apagarMotorista = async () => {
    try {
      const response = await axios.delete(`https://localhost:7204/api/Motorista/${motoristaselecionado.id}`)
        
        if (response.status === 201 || response.status === 200 || response.status === 204) { 
          Swal.fire({
            icon: 'success',
            title: 'Motorista apagado com sucesso!'
          });
          buscarmotoristas()  
          editModal.value = false; 
          console.log("numero de motoristas: ", motoristas.length    ,motoristas)
          count--
          if (count < 1) {
            window.location.reload()
          }
        }
      } catch (error) {
        console.error('Erro ao deletar motorista: ', error);
      }
  };

  onMounted(() => {
      buscarmotoristas();
    });

    console.log('contador: ', count)

</script>
<template>
  <div class="flex w-full gap-8"  >
    <Sidebar />
    <div class="p-7  w-full">
      <div class="client__header">
        <div class="flex justify-around items-center">
          <h1 class=" text-xl font-bold text-gray-400">Lista de Motoristas</h1>
          <button class="modal__btn" @click="showModal = true">
            <div class="flex items-center">
              <img class="mr-2" src="/src/assets/svg/add-profile.svg" alt="">
              <span>Adicionar Motorista</span>
            </div>
          </button>
        </div>
      </div>
      <div class="client__body">
        <div class="container mx-auto py-8">
          <div class="" v-if="motoristas">
            <table class="w-full border-collapse border border-gray-200 rounded">
              <thead>
                <tr class="bg-gray-100">
                  <th class="py-2 px-4 border border-gray-200">Nome</th>
                  <th class="py-2 px-4 border border-gray-200">Email</th>
                  <th class="py-2 px-4 border border-gray-200">Telefone</th>
                  <th class="py-2 px-4 border border-gray-200">Tipo Veiculo</th>
                  <th class="py-2 px-4 border border-gray-200">Placa Veiculo</th>
                  <th class="py-2 px-4 border border-gray-200">Cidade/Estado  </th>
                  <th class="py-2 px-4 border border-gray-200">Ações</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(motorista, index) in motoristas" :key="index" class="bg-white">
                  <td class="py-2 px-4 border border-gray-200">{{ motorista.nome }}</td>
                  <td class="py-2 px-4 border border-gray-200">{{ motorista.email }}</td>
                  <td class="py-2 px-4 border border-gray-200">{{ motorista.telefone }}</td>
                  <td class="py-2 px-4 border border-gray-200">{{ motorista.tipoVeiculo }}</td>
                  <td class="py-2 px-4 border border-gray-200">{{ motorista.placaVeiculo }}</td>
                  <td class="py-2 px-4 border border-gray-200">{{ motorista.cidade }}/{{ motorista.estado }}</td>
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
    <h2 class="text-center text-lg mb-8">Cadastrar Motorista</h2>
    <div class="grid grid-cols-2 gap-4">
      <div class="input__box mb-2">
        <label for="nome" class="block w-full">Nome:</label>
        <input v-model="novoMotorista.nome" class="w-full h-[40px] p-2 border rounded mt-2" name="nome" type="text" placeholder="Digite o nome">
      </div>
      <div class="input__box mb-2">
        <label for="doc" class="block w-full">CPF/CNPJ:</label>
        <input v-model="novoMotorista.cpf" class="w-full h-[40px] p-2 border rounded mt-2" name="doc" type="text" placeholder="Digite o CPF/CNPJ">
      </div>
      <div class="input__box">
        <label for="telefone" class="block w-full">Telefone:</label>
        <input v-model="novoMotorista.telefone" class="w-full h-[40px] p-2 border rounded mt-2" name="telefone" type="text" placeholder="Digite o telefone">
      </div>
      <div class="input__box">
          <label for="tipoVeiculo" class="block w-full">Tipo Veiculo:</label>
          <select v-model="novoMotorista.tipoVeiculo" class="w-full h-[40px] p-2 border rounded mt-2" name="tipoVeiculo" id="tipoVeiculo">
              <option value="" disabled selected>Selecione um veículo</option>
              <option value="Moto">Moto</option>
              <option value="Carro">Carro</option>
              <option value="Picape">Picape</option>
              <option value="Van">Van</option>
              <option value="Caminhão">Caminhão</option>
          </select>
      </div>
      <div class="input__box">
        <label for="telefone" class="block w-full">Placa Veiculo:</label>
        <input v-model="novoMotorista.placaVeiculo" class="w-full h-[40px] p-2 border rounded mt-2" name="placaVeiculo" type="text" placeholder="Digite a Placa">
      </div>
      <div class="input__box">
        <label for="telefone" class="block w-full">Telefone:</label>
        <input v-model="novoMotorista.telefone" class="w-full h-[40px] p-2 border rounded mt-2" name="telefone" type="text" placeholder="Digite o telefone">
      </div>
      <div class="input__box">
        <label for="email" class="block w-full">E-mail:</label>
        <input v-model="novoMotorista.email" class="w-full h-[40px] p-2 border rounded mt-2" name="email" type="text" placeholder="Digite o E-mail">
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
        <input v-model="novoMotorista.numero" class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="numero" type="text" placeholder="Digite o número">
      </div>
      
    </div>
    <button class="close__btn" @click="showModal = false">
      <svg width="30px" height="30px" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg" fill="none"><path stroke="#000000" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 12 7 7m5 5 5 5m-5-5 5-5m-5 5-5 5"/></svg>
    </button>
    <button @click="adicionarMotorista" class="w-full h-[40px] bg-[#b91c1c] rounded mt-4 text-white">
      Cadastrar
    </button>
  </Modal>
  
  <Modal :show="editModal">
    <h2 class="text-center text-lg mb-8">Editar {{ motoristaselecionado.nome }}</h2>
    <div class="grid grid-cols-2 gap-4">
      <div class="input__box mb-2">
        <label for="nome" class="block w-full">Nome:</label>
        <input class="w-full h-[40px] p-2 border rounded mt-2" name="nome" type="text" v-model="motoristaselecionado.nome" placeholder="Digite o nome">
      </div>
      <div class="input__box mb-2">
        <label for="doc" class="block w-full">CPF/CNPJ:</label>
        <input class="w-full h-[40px] p-2 border rounded mt-2" name="doc" type="text" v-model="motoristaselecionado.cpf" placeholder="Digite o CPF/CNPJ">
      </div>
      <div class="input__box">
        <label for="doc" class="block w-full">Telefone:</label>
        <input class="w-full h-[40px] p-2 border rounded mt-2" name="doc" type="text" v-model="motoristaselecionado.telefone" placeholder="Digite o telefone">
      </div>
      <div class="input__box">
          <label for="tipoVeiculo" class="block w-full">Tipo Veiculo:</label>
          <select v-model="motoristaselecionado.tipoVeiculo" class="w-full h-[40px] p-2 border rounded mt-2" name="tipoVeiculo" id="tipoVeiculo">
              <option value="" disabled selected>Selecione um veículo</option>
              <option value="Moto">Moto</option>
              <option value="Carro">Carro</option>
              <option value="Picape">Picape</option>
              <option value="Van">Van</option>
              <option value="Caminhão">Caminhão</option>
          </select>
      </div>
      <div class="input__box">
        <label for="telefone" class="block w-full">Placa Veiculo:</label>
        <input v-model="motoristaselecionado.placaVeiculo" class="w-full h-[40px] p-2 border rounded mt-2" name="placaVeiculo" type="text" placeholder="Digite a Placa">
      </div>
      <div class="input__box">
        <label for="email" class="block w-full">E-mail:</label>
        <input class="w-full h-[40px] p-2 border rounded mt-2" name="email" type="text" v-model="motoristaselecionado.email" placeholder="Digite o E-mail">
      </div>
      <div class="input__box">
        <label for="cep" class="block w-full">CEP:</label>
        <input class="w-full h-[40px] p-2 border rounded mt-2" name="cep" type="text" v-model="motoristaselecionado.cep" placeholder="Digite o CEP">
      </div>
      <div class="input__box">
        <label for="logradouro" class="block w-full">Logradouro:</label>
        <input class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="logradouro" v-model="motoristaselecionado.logradouro" type="text" placeholder="Digite o logradouro">
      </div>
      <div class="input__box">
        <label for="bairro" class="block w-full">Bairro:</label>
        <input class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="bairro" type="text" v-model="motoristaselecionado.bairro" placeholder="Digite o bairro">
      </div>
      <div class="input__box">
        <label for="complemento" class="block w-full">Cidade:</label>
        <input class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="complemento" type="text" v-model="motoristaselecionado.cidade" placeholder="Digite o complemento">
      </div>
      <div class="input__box">
        <label for="estado" class="block w-full">Estado:</label>
        <input class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="estado" type="text" v-model="motoristaselecionado.estado" placeholder="Digite o estado">
      </div>
      <div class="input__box">
        <label for="numero" class="block w-full">Número:</label>
        <input class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="numero" type="text" v-model="motoristaselecionado.numero" placeholder="Digite o número">
      </div>
      
    </div>
    <button class="close__btn" @click="editModal = false">
      <svg width="30px" height="30px" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg" fill="none"><path stroke="#000000" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 12 7 7m5 5 5 5m-5-5 5-5m-5 5-5 5"/></svg>
    </button>
    <button class="w-full h-[40px] bg-[#006400] rounded mt-4 text-white" @click="editarMotorista()">
      Editar
    </button>
  </Modal>
  <Modal :show="deleteModal">
    <h2 class="text-center text-lg mb-8">Deletar {{ motoristaselecionado.nome }}</h2>
    <p class="text-center">Tem certeza que deseja deletar o motorista: {{ motoristaselecionado.nome }} ?</p>
    <button class="close__btn" @click="deleteModal = false">
      <svg width="30px" height="30px" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg" fill="none"><path stroke="#000000" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 12 7 7m5 5 5 5m-5-5 5-5m-5 5-5 5"/></svg>
    </button>
    <button class="w-full h-[40px] bg-[#b91c1c] rounded mt-4 text-white" @click="apagarMotorista(), deleteModal = false">
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
