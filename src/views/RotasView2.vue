<script setup>
import { ref, onMounted } from 'vue';
import Sidebar from '../components/Sidebar.vue';

onMounted(() => {
    initializeMap();
});

let map;
const initializeMap = () => {
    const platform = new H.service.Platform({
        apikey: 'eGEbMqmjPEdw473hAUUXR5t_22Ys36iC6n4NfKGCu8Q'
    });

    const defaultLayers = platform.createDefaultLayers();

    map = new H.Map(
        document.getElementById('mapContainer'),
        defaultLayers.vector.normal.map,
        {
            zoom: 6,
            center: { lat: -14.235, lng: -51.9253 },
        }
    );

    const router = platform.getRoutingService(null, 8);

    const params = {
        representation: 'display',
        transportMode: 'car',
        origin: '50.5160,13.377',
        destination: '52.5206,13.3862',
        return: 'polyline,summary',
    };

    router.calculateRoute(params, onSuccess, onError);
};

const onSuccess = (result) => {
    const route = result.routes[0];
    addRouteToMap(route);
};

const onError = (error) => {
    console.error('Erro ao calcular rota:', error);
};

const addRouteToMap = (route) => {
    const routeShape = route.sections[0].polyline;
    const linestring = H.geo.LineString.fromFlexiblePolyline(routeShape);

    const routeLine = new H.map.Polyline(linestring, {
        style: { strokeColor: 'blue', lineWidth: 5 }
    });

    map.addObject(routeLine);
    map.getViewModel().setLookAtData({ bounds: routeLine.getBoundingBox() });
};
</script>

<template>
    <div class="flex w-full gap-8">
        <Sidebar/>
        <div class="p-7 w-full">
            <div class="client__header">
                <div class="flex justify-around items-center">
                    <h1 class="text-xl font-bold text-gray-400">Lista de Rotas</h1>
                </div>
            </div>
            <div class="client__body">
                <div class="container w-full mx-auto py-8">
                    <div id="mapContainer" class="map-container"></div>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
.map-container {
    height: 500px;
}
.close__btn {
    position: absolute;
    top: 20px;
    right: 20px;
}
</style>
