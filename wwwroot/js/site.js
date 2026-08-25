document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('search-form');
    const cityInput = document.getElementById('city-input');

    const emptyState = document.getElementById('empty-state');
    const loadingState = document.getElementById('loading-state');
    const errorState = document.getElementById('error-state');
    const results = document.getElementById('results');

    form.addEventListener('submit', function (event) {
        event.preventDefault();
        const city = cityInput.value.trim();

        if (!city) {
            showError('Please enter a city name.');
            return;
        }

        searchWeather(city);
    });

    async function searchWeather(city) {
        showLoading();

        try {
            const response = await fetch(`/Weather/Search?city=${encodeURIComponent(city)}`);

            if (!response.ok) {
                showError('Unable to retrieve weather data. Please try again later.');
                return;
            }

            const data = await response.json();

            if (!data.success) {
                showError(data.errorMessage || 'Unable to retrieve weather data. Please try again later.');
                return;
            }

            renderWeather(data);
        } catch (err) {
            showError('Unable to retrieve weather data. Please try again later.');
        }
    }

    function renderWeather(data) {
        document.getElementById('city-name').textContent = `${data.city}, ${data.country}`;
        document.getElementById('condition-text').textContent = data.condition;
        document.getElementById('weather-icon').src = data.iconUrl;
        document.getElementById('temperature').textContent = `${Math.round(data.temperature)}°C`;
        document.getElementById('feels-like').textContent = `${Math.round(data.feelsLike)}°C`;
        document.getElementById('humidity').textContent = `${data.humidity}%`;
        document.getElementById('wind-speed').textContent = `${data.windSpeed} km/h`;
        document.getElementById('pressure').textContent = `${data.pressure} hPa`;
        document.getElementById('sunrise').textContent = data.sunrise;
        document.getElementById('sunset').textContent = data.sunset;

        const forecastContainer = document.getElementById('forecast-container');
        forecastContainer.innerHTML = '';

        data.forecast.forEach(day => {
            const col = document.createElement('div');
            col.className = 'col-6 col-md';
            col.innerHTML = `
                <div class="forecast-card">
                    <div class="forecast-day">${day.day}</div>
                    <img src="${day.iconUrl}" alt="${day.condition}" width="50" height="50" />
                    <div class="fw-bold">${Math.round(day.temperature)}°C</div>
                    <div class="forecast-condition">${day.condition}</div>
                </div>
            `;
            forecastContainer.appendChild(col);
        });

        hideAll();
        results.classList.remove('d-none');
    }

    function showLoading() {
        hideAll();
        loadingState.classList.remove('d-none');
    }

    function showError(message) {
        hideAll();
        errorState.textContent = message;
        errorState.classList.remove('d-none');
    }

    function hideAll() {
        emptyState.classList.add('d-none');
        loadingState.classList.add('d-none');
        errorState.classList.add('d-none');
        results.classList.add('d-none');
    }
});