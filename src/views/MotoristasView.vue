<script setup>
  import Sidebar from '../components/Sidebar.vue'
  import Swal from 'sweetalert2';
  import Modal from '../components/Modal.vue';
  import Modal2 from '../components/Modal2.vue';
  import { ref, onMounted } from "vue";
  import axios from "axios";

  const showModal = ref(false);
  const editModal = ref(false);
  const deleteModal = ref(false);
  const cepValido = ref(false); 
  const telefoneValido = ref(false); 
  const cpfValido = ref(false);
  const numeroValido = ref(false); 
  const nomeValido = ref(false); 
  const emailValido = ref(false);
  const placaValido = ref(false);
  const veiculoValido = ref(false);
  const motoristaBusca = ref('');
  const cep = ref('');
  const endereco = ref({logradouro: '',bairro: '',cidade: '',estado: ''});
  const novoMotorista = ref({nome: '',cpf: '',telefone: '',email: '',tipoVeiculo: '',placaVeiculo: '',cep: '',logradouro: '',bairro: '',estado: '',numero: '',complemento: ''});
  let motoristaselecionado = ref({nome: '',email: '',tipoVeiculo: '',placaVeiculo: '',cpf: '',telefone: '',logradouro: '',bairro: '', cep: '',numero: '',cidade: '',estado: ''});
  let motoristas = ref('')



  const verificarCPF = () => {
    const documentoLimpo = novoMotorista.value.cpf.replace(/\D/g, '');
    if (documentoLimpo.length === 11) {
      novoMotorista.value.cpf = documentoLimpo.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');
        if (/^(\d)\1{10}$/.test(documentoLimpo)) {
            cpfValido.value = false;
        }
        let soma = 0;
        for (let i = 0; i < 9; i++) {
            soma += parseInt(documentoLimpo.charAt(i)) * (10 - i);
        }
        let resto = 11 - (soma % 11);
        let digitoVerificador1 = resto === 10 || resto === 11 ? 0 : resto;

        soma = 0;
        for (let i = 0; i < 10; i++) {
            soma += parseInt(documentoLimpo.charAt(i)) * (11 - i);
        }
        resto = 11 - (soma % 11);
        let digitoVerificador2 = resto === 10 || resto === 11 ? 0 : resto;

        if (parseInt(documentoLimpo.charAt(9)) !== digitoVerificador1 || parseInt(documentoLimpo.charAt(10)) !== digitoVerificador2) {
            cpfValido.value = false;
        }
        cpfValido.value = true;
    }
    else if (documentoLimpo.length === 14) {
        novoCliente.value.cpf = documentoLimpo.replace(/(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/, '$1.$2.$3/$4-$5');
        
        let multiplicadoresPrimeiroDigito = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        let multiplicadoresSegundoDigito = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        
        let soma = 0;
        
        for (let i = 0; i < 12; i++) {
            soma += parseInt(documentoLimpo.charAt(i)) * multiplicadoresPrimeiroDigito[i];
        }
        
        let resto = soma % 11;
        let digitoVerificador1 = resto < 2 ? 0 : 11 - resto;
        
        soma = 0;
        
        for (let i = 0; i < 13; i++) {
            soma += parseInt(documentoLimpo.charAt(i)) * multiplicadoresSegundoDigito[i];
        }
        
        resto = soma % 11;
        let digitoVerificador2 = resto < 2 ? 0 : 11 - resto;

        if (parseInt(documentoLimpo.charAt(12)) !== digitoVerificador1 || parseInt(documentoLimpo.charAt(13)) !== digitoVerificador2) {
          cpfValido.value = false;
        }
        
        cpfValido.value = true;
    } else {
        cpfValido.value = false;
    }
  };
  
  const verificarEmail = () => {
      const email = novoMotorista.value.email.trim();

      if (email.length === 0) {
          emailValido.value = false;
          return;
      }

      const regexEmail = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

      if (regexEmail.test(email)) {
          emailValido.value = true;
      } else {
          emailValido.value = false;
      }
  };

  const verificarNome = () => {
      const nomeCompleto = novoMotorista.value.nome.trim();
      if (nomeCompleto.length === 0) {
          nomeValido.value = false;
          return;
      }
      const partesNome = nomeCompleto.split(" ");
      if (partesNome.length < 2) {
          nomeValido.value = false;
          return;
      }
      const nomeFormatado = partesNome.map((parte) => parte.charAt(0).toUpperCase() + parte.slice(1).toLowerCase()).join(" ");
      novoMotorista.value.nome = nomeFormatado;
      nomeValido.value = true;
  };

  const verificarTelefone = () => {
    const telefoneLimpo = novoMotorista.value.telefone.replace(/\D/g, '');
    novoMotorista.value.telefone = telefoneLimpo.replace(/^(\d{2})(\d{4,5})(\d{4})$/, '($1) $2-$3');
    if  (telefoneLimpo.length > 9){
      telefoneValido.value = true;
    }else{
      telefoneValido.value = false;
    }
  };

  const verificarNumero = () => {
      const numero = novoMotorista.value.numero.trim();
      if (numero.length > 0) {
          numeroValido.value = true;
      } else {
          numeroValido.value = false;
      }
  };

  const verificarVeiculo = () => {
    const veiculo = novoMotorista.value.tipoVeiculo;
    if (veiculo == '') {
      veiculoValido.value = false;
    } else {
      veiculoValido.value = true;
    }
  };
    
  const verificarPlaca = () => {
    const placaLimpa = novoMotorista.value.placaVeiculo.replace(/[^a-zA-Z0-9]/g, '').toUpperCase();
    const oldPattern = /^[A-Z]{3}[0-9]{4}$/;
    const mercosulPattern = /^[A-Z]{3}[0-9][A-Z][0-9]{2}$/;
    if (oldPattern.test(placaLimpa)) {
        novoMotorista.value.placaVeiculo = placaLimpa.replace(/^([A-Z]{3})([0-9]{4})$/, '$1-$2');
        placaValido.value = true;
    } else if (mercosulPattern.test(placaLimpa)) {
        novoMotorista.value.placaVeiculo = placaLimpa.replace(/^([A-Z]{3})([0-9])([A-Z])([0-9]{2})$/, '$1$2$3$4');
        placaValido.value = true;
    } else {
        placaValido.value = false;
    }
  };
 
  const buscarEndereco = async () => {
    const cepLimpo = cep.value.replace(/\D/g, '');
    if (cepLimpo.length !== 8) {
      limparEndereco();
      cepValido.value = false;
      return;
    }
    cep.value = cep.value.replace(/\D/g, '').replace(/^(\d{5})(\d)/, '$1-$2');
    const inputs = document.querySelectorAll('.input__adress')
    try {
      const response = await axios.get(`https://api.postmon.com.br/v1/cep/${cep.value}`);
      
      if (response.data && Object.keys(response.data).length !== 0) {
        endereco.value = response.data;
        cepValido.value = true;
      } else {
        inputs.forEach(element => { 
          element.value = "";
          cepValido.value = true;
        });
        limparEndereco()
        cepValido.value = false;
      }
      
    } catch (error) {
      limparEndereco()
      cepValido.value = false;
    }
  };

  const limparEndereco = () => {
    endereco.value = {
      logradouro: '',
      bairro: '',
      cidade: '',
      estado: ''
    };
    cepValido.value = false;
  };

  const buscarMotoristas = async () => {
  try {
    motoristas.value = '';
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
  }

  const pegarMotoristasNome = async (nome) => {
    if (nome == '') {   
      try {
      motoristas.value = '';
      const response = await axios.get(`https://localhost:7204/api/Motorista`);
      if (response.data && Object.keys(response.data).length !== 0) {
        motoristas.value = response.data;
      } 
    } catch (error) {
      console.error('Erro ao pegar motoristas: ', error);
    }
    }else{
      try {
        motoristas.value = '';
        const response = await axios.get(`https://localhost:7204/api/Motorista/b=${nome}`);
        if (response.data && Object.keys(response.data).length !== 0) {
          motoristas.value = response.data;
        } 
      } catch (error) {
        console.error('Erro ao pegar motoristas: ', error);
      }
    } 
  };

  const buscarNome = async () => {
    try {
      await pegarMotoristasNome(motoristaBusca.value);
    } catch (error) {
      console.error('Erro ao buscar os motoristas: ', error);
    }
  };

  const veiculoIconMap = {
    Carro: '/src/assets/svg/carro.svg',
    Caminhão: '/src/assets/svg/caminhao.svg',
    Van: '/src/assets/svg/van.svg',
    Picape: '/src/assets/svg/picape.svg',
    Moto: '/src/assets/svg/moto.svg'
  };

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
          position: "top",
          icon: "success",
          title: "Motorista cadastrado com sucesso!",
          showConfirmButton: false,
          timer: 1500,
          backdrop: false
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
          cidade: '',
          estado: ''
        };
        cep.value = ""
        showModal.value = false; 
        cepValido.value = false;  
        telefoneValido.value = false; 
        cpfValido.value = false; 
        numeroValido.value = false; 
        nomeValido.value = false; 
        emailValido.value = false; 
        veiculoValido.value = false; 
        placaValido.value = false; 
        buscarMotoristas()
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
            position: "top",
            icon: "success",
            title: "Motorista editado com sucesso!",
            showConfirmButton: false,
            timer: 1500,
            backdrop: false
          });
          editModal.value = false; 
          buscarMotoristas()
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
            position: "top",
            icon: "success",
            title: "Motorista apagado com sucesso!",
            showConfirmButton: false,
            timer: 1500,
            backdrop: false
          });
          buscarMotoristas()  
          editModal.value = false; 
        }
      } catch (error) {
        console.error('Erro ao deletar motorista: ', error);
      }
  };

  onMounted(() => {
    buscarMotoristas();
  });

</script>


<template>
  <div class="flex w-full gap-8"  >
    <Sidebar />
    <div class="p-7  w-full">
      <div class="client__header bg-gray-100 py-4 px-8 rounded-lg shadow-md transition-all duration-300 hover:shadow-lg">
        <div class="flex justify-between items-center">
          <h1 class="text-2xl font-bold text-gray-700">Motoristas</h1>
          <div class="flex items-center">
            <input type="text" v-model="motoristaBusca" @input="buscarNome" placeholder="Pesquisar por nome..." class="bg-white border border-gray-300 rounded-md px-4 py-2 focus:outline-none focus:border-red-600 transition-colors duration-300">
            <button class="modal__btn bg-red-600 text-white font-semibold px-4 py-2 ml-2 rounded-lg flex items-center hover:bg-red-700 transition-all duration-300 transform hover:scale-105" @click="showModal = true">
              <img class="mr-2" src="/src/assets/svg/addb.svg" alt="Adicionar Motorista">
              <span>Adicionar Motorista</span>
            </button>
          </div>
        </div>
      </div>
      <div class="client__body">
        <div class="container mx-auto py-8">
          <div v-if="motoristas" class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
            <div v-for="(motorista, index) in motoristas" :key="index" class="bg-white shadow-md rounded-lg overflow-hidden transform transition duration-300 hover:shadow-xl hover:scale-105">
              <div class="p-6">
                <img src="/src/assets/img/user3.png" alt="Foto da Motorista" class="w-24 h-24 rounded-full mx-auto transition duration-300 hover:scale-110">
                <h3 class="text-center text-lg font-bold mt-4">{{ motorista.nome }}</h3>
                <p class="text-center text-gray-600">{{ motorista.cidade }}/{{ motorista.estado }}</p>
                <div class="mt-4 text-center">
                  <p>{{ motorista.email }}</p>
                  <p>{{ motorista.telefone }}</p>
                  <img :src="veiculoIconMap[motorista.tipoVeiculo]" alt="Tipo de Veículo" class="w-20 h-20 mx-auto mt-2" />
                  <p>{{ motorista.tipoVeiculo }} ({{ motorista.placaVeiculo }})</p>
                </div>
                <div class="border-t mt-4 pt-4 flex justify-around">
                  <button @click="editModal = true, dataForm(index)" class="text-gray-700 hover:text-blue-500 transform transition duration-300 hover:scale-125">
                    <img src="/src/assets/svg/editar.svg" alt="Editar" title="Editar" class="w-7 h-7">
                  </button>
                  <button @click="dataForm(index), deleteModal = true" class="text-gray-700 hover:text-red-500 transform transition duration-300 hover:scale-125">
                    <img src="/src/assets/svg/lixo.svg" alt="Apagar" title="Apagar" class="w-7 h-7">
                  </button>
                </div>
              </div>
            </div>
          </div>
          <div v-else>
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
    <div class="grid grid-cols-3 gap-4">
      <div class="input__box mb-2">
        <label for="nome" class="block w-full">Nome:</label>
        <input v-model="novoMotorista.nome":class="{ 'border-green-500': nomeValido, 'border-red-500': !nomeValido }" class="w-full h-[40px] p-2 border rounded mt-2" name="nome" type="text" placeholder="Nome Sobrenome"@input="verificarNome"maxlength="30">
      </div>
      <div class="input__box mb-2">
        <label for="doc" class="block w-full">CPF/CNPJ:</label>
        <input v-model="novoMotorista.cpf":class="{ 'border-green-500': cpfValido, 'border-red-500': !cpfValido }" class="w-full h-[40px] p-2 border rounded mt-2" name="doc" type="text" placeholder="000.000.000-00"@input="verificarCPF" maxlength="18">
      </div>
      <div class="input__box">
        <label for="telefone" class="block w-full">Telefone:</label>
        <input v-model="novoMotorista.telefone":class="{ 'border-green-500': telefoneValido, 'border-red-500': !telefoneValido }" class="w-full h-[40px] p-2 border rounded mt-2" name="telefone" type="text" placeholder="(00) 00000-0000"@input="verificarTelefone" maxlength="15">
      </div>
      <div class="input__box">
          <label for="tipoVeiculo" class="block w-full">Tipo Veiculo:</label>
          <select v-model="novoMotorista.tipoVeiculo":class="{ 'border-green-500': veiculoValido, 'border-red-500': !veiculoValido }" class="w-full h-[40px] p-2 border rounded mt-2" name="tipoVeiculo" id="tipoVeiculo"@change="verificarVeiculo">
              <option value="" disabled selected>Selecione um veículo</option>
              <option value="Moto">Moto - 20KG</option>
              <option value="Carro">Carro - 100KG</option>
              <option value="Picape">Picape - 200KG</option>
              <option value="Van">Van - 500KG</option>
              <option value="Caminhão">Caminhão - 2000KG</option>
          </select>
      </div>
      <div class="input__box">
        <label for="telefone" class="block w-full">Placa Veiculo:</label>
        <input v-model="novoMotorista.placaVeiculo":class="{ 'border-green-500': placaValido, 'border-red-500': !placaValido }" class="w-full h-[40px] p-2 border rounded mt-2" name="placaVeiculo" type="text" placeholder="AAA-0000"@input="verificarPlaca" maxlength="8">
      </div>
      <div class="input__box">
        <label for="email" class="block w-full">E-mail:</label>
        <input v-model="novoMotorista.email":class="{ 'border-green-500': emailValido, 'border-red-500': !emailValido }" class="w-full h-[40px] p-2 border rounded mt-2" name="email" type="text" placeholder="email@dominio"@input="verificarEmail" maxlength="30">
      </div>
      <div class="input__box">
        <label for="cep" class="block w-full">CEP:</label>
        <input v-model="cep":class="{ 'border-green-500': cepValido, 'border-red-500': !cepValido }" @input="buscarEndereco" class="w-full h-[40px] p-2 border rounded mt-2" name="cep" type="text" placeholder="00000-000"maxlength="9">
      </div>
      <div class="input__box">
        <label for="numero" class="block w-full">Número:</label>
        <input v-model="novoMotorista.numero":class="{ 'border-green-500': numeroValido, 'border-red-500': !numeroValido }" class="input__adress w-full h-[40px] p-2 border rounded mt-2" name="numero" type="text" placeholder="0"@input="verificarNumero" maxlength="10">
      </div> 
    </div>
    <div class="text-center text-base2 mt-4" style="max-height: 40px; min-height: 40px;">
        <label v-if="(cepValido)" class="block w-full">{{endereco.logradouro}}, {{endereco.bairro}} - {{endereco.cidade}}/{{endereco.estado}}</label>
      </div>
    <button class="close__btn" @click="showModal = false">
      <svg width="30px" height="30px" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg" fill="none"><path stroke="#000000" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 12 7 7m5 5 5 5m-5-5 5-5m-5 5-5 5"/></svg>
    </button>
    <button @click="adicionarMotorista" class="w-full h-[40px] bg-[#b91c1c] rounded mt-4 text-white" :disabled="!(nomeValido && cpfValido && telefoneValido && emailValido && cepValido && numeroValido && placaValido && veiculoValido)">
      Cadastrar
      <span v-if="!(nomeValido && cpfValido && telefoneValido && emailValido && placaValido && veiculoValido && cepValido && numeroValido)" class="text-sm ml-2 text-gray-400">(Preencha todos os campos corretamente)</span>
    </button>
  </Modal>
  
  <Modal :show="editModal">
    <h2 class="text-center text-lg mb-8">Editar {{ motoristaselecionado.nome }}</h2>
    <div class="grid grid-cols-3 gap-4">
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
  <Modal2 :show="deleteModal">
    <h2 class="text-center text-lg mb-8">Deletar {{ motoristaselecionado.nome }}</h2>
    <p class="text-center">Tem certeza que deseja deletar o motorista?</p>
    <button class="close__btn" @click="deleteModal = false">
      <svg width="30px" height="30px" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg" fill="none"><path stroke="#000000" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 12 7 7m5 5 5 5m-5-5 5-5m-5 5-5 5"/></svg>
    </button>
    <button class="w-full h-[40px] bg-[#b91c1c] rounded mt-4 text-white" @click="apagarMotorista(), deleteModal = false">
      Deletar
    </button>
  </Modal2>
</template>

<style scoped>
  .close__btn {
    position: absolute;
    top: 20px;
    right: 20px;
  }
</style>
