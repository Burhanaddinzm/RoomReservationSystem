"use strict";

const form = document.querySelector("form");

const validateParticipantCount = (event) => {
  const select = form.querySelector("#participant-select");
  const roomSize = parseInt(select.dataset.roomSize, 10);

  if (isNaN(roomSize)) {
    console.error("Invalid room size");
    event.preventDefault();
    return;
  }

  let optionCount = 0;

  for (let i = 0; i < select.options.length; i++) {
    if (select.options[i].selected) {
      optionCount++;
    }
  }

  if (optionCount === 0) {
    alert("Please select at least one participant.");
    event.preventDefault();
    return;
  }

  if (optionCount > roomSize) {
    alert("Too many participants selected.");
    event.preventDefault();
    return;
  }
};

document.addEventListener("DOMContentLoaded", () => {
  const startDateInput = document.getElementById("start-date");
  const endDateInput = document.getElementById("end-date");

  const roundToNext30Minutes = (date) => {
    const ms = 1000 * 60 * 30;
    return new Date(Math.ceil(date.getTime() / ms) * ms);
  };

  const formatDateTimeLocal = (date) => {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const day = String(date.getDate()).padStart(2, "0");
    const hours = String(date.getHours()).padStart(2, "0");
    const minutes = String(date.getMinutes()).padStart(2, "0");
    return `${year}-${month}-${day}T${hours}:${minutes}`;
  };

  const now = new Date();
  const roundedNow = roundToNext30Minutes(now);

  const minDateTime = formatDateTimeLocal(roundedNow);
  startDateInput.min = minDateTime;
  endDateInput.min = minDateTime;

  startDateInput.value = minDateTime;

  startDateInput.addEventListener("change", () => {
    const selectedStartDate = new Date(startDateInput.value);

    if (selectedStartDate < roundedNow) {
      startDateInput.value = minDateTime;
    } else {
      const roundedStartDate = roundToNext30Minutes(selectedStartDate);
      startDateInput.value = formatDateTimeLocal(roundedStartDate);
    }

    const minEndDate = new Date(startDateInput.value);
    minEndDate.setMinutes(minEndDate.getMinutes() + 30);
    endDateInput.min = formatDateTimeLocal(minEndDate);

    const selectedEndDate = new Date(endDateInput.value);
    if (selectedEndDate < minEndDate) {
      endDateInput.value = formatDateTimeLocal(minEndDate);
    }
  });

  endDateInput.addEventListener("change", () => {
    const selectedEndDate = new Date(endDateInput.value);
    const selectedStartDate = new Date(startDateInput.value);

    if (selectedEndDate <= selectedStartDate) {
      const minEndDate = new Date(selectedStartDate);
      minEndDate.setMinutes(minEndDate.getMinutes() + 30);
      endDateInput.value = formatDateTimeLocal(minEndDate);
    } else {
      const roundedEndDate = roundToNext30Minutes(selectedEndDate);
      endDateInput.value = formatDateTimeLocal(roundedEndDate);
    }
  });
});

form.addEventListener("submit", validateParticipantCount);
