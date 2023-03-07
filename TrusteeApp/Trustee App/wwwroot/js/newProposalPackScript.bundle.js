var newProposalPackScript;(()=>{var e={940:function(e){"undefined"!=typeof self&&self,e.exports=(()=>{"use strict";var e={d:(t,n)=>{for(var r in n)e.o(n,r)&&!e.o(t,r)&&Object.defineProperty(t,r,{enumerable:!0,get:n[r]})},o:(e,t)=>Object.prototype.hasOwnProperty.call(e,t),r:e=>{"undefined"!=typeof Symbol&&Symbol.toStringTag&&Object.defineProperty(e,Symbol.toStringTag,{value:"Module"}),Object.defineProperty(e,"__esModule",{value:!0})}},t={};e.r(t),e.d(t,{util:()=>n});const n=(()=>{function e(e){return null==e||null==e||0==e.trim().length}return{getQueryParameter:function(e,t){let n="?"+t,r=new RegExp("[?&]"+e+"=([^&#]*)","i").exec(n);return r?r[1]:null},navigateTo:function(e){window.location.hash="#"+e},stringIsEmptyorNullOrUndefined:e,enableButtonElement:function(e){e&&(e.disabled=!1)},disableButtonElement:function(e){e&&(e.disabled=!0)},displaySpinner:function(){const e=document.getElementById("loading-gif");e&&e.classList.remove("rds-hidden")},hideSpinner:function(){const e=document.getElementById("loading-gif");e&&e.classList.add("rds-hidden")},getElementSiblings:function(e){let t=[];if(!e.parentNode)return t;let n=e.parentNode.firstElementChild;if(n)do{n!=e&&t.push(n)}while(n=null==n?void 0:n.nextElementSibling);return t},removeClassFromSiblings:function(e,t){for(var n=0;n<e.length;n++)e[n].classList.remove(t)},addClassToSiblings:function(e,t){for(var n=0;n<e.length;n++)e[n].classList.add(t)},getArrayFromNodeList:function(e){return Array.from(e)},getSelectedDropdownValue:function(t){if(!t)return"";if(0==t.options.length)return"";const n=t.options[t.selectedIndex].value;return e(n)?"":n},highlightSelectedRow:function(e,t){const n=e.getElementsByTagName("td"),r=e.tBodies[0];for(var o=0;o<n.length;o++)n[o].onclick=function(e){const n=e.target.parentElement,o=r.getElementsByTagName("tr");for(var i=0;i<o.length;i++)o[i].classList.remove(t);n&&n.classList.add(t)}},comboboxIsEmpty:function(e){return 0==e.options.length},selectedComboboxValue:function(e){return e.options[e.selectedIndex].value},readInputElementValue:function(e){return e.value.trim()},setComboboxValue:function(e,t){if(e.options.length>0)for(var n=0;n<e.options.length;n++)if(e.options[n].value==t)return void(e.options[n].selected=!0)},setInputElementValue:function(e,t){try{e&&t.length>0&&(e.value=t)}catch(e){}},convertDateToISOFormat:function(e){const t=new Date(e),n=t.getDate().toString().padStart(2,"0"),r=(t.getMonth()+1).toString().padStart(2,"0");return t.getFullYear()+"-"+r+"-"+n},isValidEmailAddress:function(e){return!!/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(e)},formatStandardDate:function(e){const t=new Date(e),n=t.getDate().toString().padStart(2,"0"),r=["JAN","FEB","MAR","APR","MAY","JUN","JUL","AUG","SEP","OCT","NOV","DEC"][t.getMonth()],o=t.getFullYear();return t.getHours(),n+"-"+r+"-"+o},formatStringAsNumber:function(e){return e.toString().replace(/\B(?=(\d{3})+(?!\d))/g,",")},removeThousandSeparator:function(e){const t=e.replace(/\,/g,"");return parseFloat(t)},isNumeric:function(e){return/^\d*\.?\d+$/.test(e)},encodeUrl:function(e){return encodeURIComponent(e)}}})();return t})()},851:(e,t,n)=>{"use strict";Object.defineProperty(t,"__esModule",{value:!0});var r=n(940);e.exports={addScriptEventListeners:function(){document.getElementById("content-placeholder"),r.util.displaySpinner(),l&&l.addEventListener("click",(function(e){if(i&&(d=r.util.getSelectedDropdownValue(i),r.util.stringIsEmptyorNullOrUndefined(d)))return swal("Validation Error","Please select a branch from the available List","error"),!1;var t={ref:d,onSuccessCallback:function(e){a&&(a.value=e.lookupName,a.setAttribute("data-agent-code",e.lookupCode))},formData:{modalName:"Agent",columnHeaders:["Agent Name","Level","Agent Code"]},searchEndPoint:"../api/agents/find",searchQueryParameterName:"searchTerm",rowIdIndex:2,searchTermRowIndex:0};lookupService.show(t)})),a&&a.addEventListener("keyup",(function(e){"Enter"===e.key&&l&&l.click()})),i&&i.addEventListener("change",(function(e){i&&a&&i.options.length>0&&(a.value="")})),r.util.hideSpinner()},saveBtnClick:function(){o&&(u=o.value.trim(),r.util.stringIsEmptyorNullOrUndefined(u))?(swal("Validation Error","Please enter a valid value in the Title field","error"),o.focus()):i&&(d=r.util.getSelectedDropdownValue(i),r.util.stringIsEmptyorNullOrUndefined(d))?swal("Validation Error","Please select a branch from the available List","error"):a&&a.hasAttribute("data-agent-code")&&(s=a.getAttribute("data-agent-code"),r.util.stringIsEmptyorNullOrUndefined(s)||r.util.stringIsEmptyorNullOrUndefined(a.value))&&(swal("Validation Error","Please enter a valid value in the Agent Code field","error"),a.focus())}};var o=document.getElementById("Title"),i=document.getElementById("Branches"),a=document.getElementById("AgentCode"),l=(document.getElementById("btn-save"),document.getElementById("btn-agent-luv")),d="",u="",s=""}},t={},n=function n(r){var o=t[r];if(void 0!==o)return o.exports;var i=t[r]={exports:{}};return e[r].call(i.exports,i,i.exports,n),i.exports}(851);newProposalPackScript=n})();