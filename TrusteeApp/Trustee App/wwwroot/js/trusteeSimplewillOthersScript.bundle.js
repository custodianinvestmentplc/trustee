var trusteeSimplewillOthersScript;(()=>{var e={347:function(e){var t;"undefined"!=typeof self&&self,t=()=>(()=>{"use strict";var e={d:(t,n)=>{for(var r in n)e.o(n,r)&&!e.o(t,r)&&Object.defineProperty(t,r,{enumerable:!0,get:n[r]})},o:(e,t)=>Object.prototype.hasOwnProperty.call(e,t),r:e=>{"undefined"!=typeof Symbol&&Symbol.toStringTag&&Object.defineProperty(e,Symbol.toStringTag,{value:"Module"}),Object.defineProperty(e,"__esModule",{value:!0})}},t={};e.r(t),e.d(t,{dataService:()=>a});var n,r=function(e,t,n,r){return new(n||(n=Promise))((function(a,i){function o(e){try{s(r.next(e))}catch(e){i(e)}}function l(e){try{s(r.throw(e))}catch(e){i(e)}}function s(e){var t;e.done?a(e.value):(t=e.value,t instanceof n?t:new n((function(e){e(t)}))).then(o,l)}s((r=r.apply(e,t||[])).next())}))};!function(e){e.fetchSuccess="FETCH_SUCCESS",e.fetchFailed="FETCH_FAILED",e.fetchError="FETCH_ERROR"}(n||(n={}));const a=(()=>{function e(e){return r(this,void 0,void 0,(function*(){const t=yield e.json();return{type:e.ok?n.fetchSuccess:n.fetchFailed,data:t,httpStatusCode:e.status,errMessage:""}}))}function t(){return r(this,void 0,void 0,(function*(){return{type:n.fetchError,data:{},httpStatusCode:502,errMessage:"Cannot connect to the server. Please check your network connection."}}))}return{getData:function(n){return r(this,void 0,void 0,(function*(){try{const t=yield fetch(n);return yield e(t)}catch(e){return console.log(e),t()}}))},postData:function(n,a){return r(this,void 0,void 0,(function*(){try{const t=yield fetch(n,{method:"POST",body:JSON.stringify(a),headers:{"Content-Type":"application/json"}});return yield e(t)}catch(e){return console.log(e),t()}}))}}})();return t})(),e.exports=t()},940:function(e){"undefined"!=typeof self&&self,e.exports=(()=>{"use strict";var e={d:(t,n)=>{for(var r in n)e.o(n,r)&&!e.o(t,r)&&Object.defineProperty(t,r,{enumerable:!0,get:n[r]})},o:(e,t)=>Object.prototype.hasOwnProperty.call(e,t),r:e=>{"undefined"!=typeof Symbol&&Symbol.toStringTag&&Object.defineProperty(e,Symbol.toStringTag,{value:"Module"}),Object.defineProperty(e,"__esModule",{value:!0})}},t={};e.r(t),e.d(t,{util:()=>n});const n=(()=>{function e(e){return null==e||null==e||0==e.trim().length}return{getQueryParameter:function(e,t){let n="?"+t,r=new RegExp("[?&]"+e+"=([^&#]*)","i").exec(n);return r?r[1]:null},navigateTo:function(e){window.location.hash="#"+e},stringIsEmptyorNullOrUndefined:e,enableButtonElement:function(e){e&&(e.disabled=!1)},disableButtonElement:function(e){e&&(e.disabled=!0)},displaySpinner:function(){const e=document.getElementById("loading-gif");e&&e.classList.remove("rds-hidden")},hideSpinner:function(){const e=document.getElementById("loading-gif");e&&e.classList.add("rds-hidden")},getElementSiblings:function(e){let t=[];if(!e.parentNode)return t;let n=e.parentNode.firstElementChild;if(n)do{n!=e&&t.push(n)}while(n=null==n?void 0:n.nextElementSibling);return t},removeClassFromSiblings:function(e,t){for(var n=0;n<e.length;n++)e[n].classList.remove(t)},addClassToSiblings:function(e,t){for(var n=0;n<e.length;n++)e[n].classList.add(t)},getArrayFromNodeList:function(e){return Array.from(e)},getSelectedDropdownValue:function(t){if(!t)return"";if(0==t.options.length)return"";const n=t.options[t.selectedIndex].value;return e(n)?"":n},highlightSelectedRow:function(e,t){const n=e.getElementsByTagName("td"),r=e.tBodies[0];for(var a=0;a<n.length;a++)n[a].onclick=function(e){const n=e.target.parentElement,a=r.getElementsByTagName("tr");for(var i=0;i<a.length;i++)a[i].classList.remove(t);n&&n.classList.add(t)}},comboboxIsEmpty:function(e){return 0==e.options.length},selectedComboboxValue:function(e){return e.options[e.selectedIndex].value},readInputElementValue:function(e){return e.value.trim()},setComboboxValue:function(e,t){if(e.options.length>0)for(var n=0;n<e.options.length;n++)if(e.options[n].value==t)return void(e.options[n].selected=!0)},setInputElementValue:function(e,t){try{e&&t.length>0&&(e.value=t)}catch(e){}},convertDateToISOFormat:function(e){const t=new Date(e),n=t.getDate().toString().padStart(2,"0"),r=(t.getMonth()+1).toString().padStart(2,"0");return t.getFullYear()+"-"+r+"-"+n},isValidEmailAddress:function(e){return!!/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(e)},formatStandardDate:function(e){const t=new Date(e),n=t.getDate().toString().padStart(2,"0"),r=["JAN","FEB","MAR","APR","MAY","JUN","JUL","AUG","SEP","OCT","NOV","DEC"][t.getMonth()],a=t.getFullYear();return t.getHours(),n+"-"+r+"-"+a},formatStringAsNumber:function(e){return e.toString().replace(/\B(?=(\d{3})+(?!\d))/g,",")},removeThousandSeparator:function(e){const t=e.replace(/\,/g,"");return parseFloat(t)},isNumeric:function(e){return/^\d*\.?\d+$/.test(e)},encodeUrl:function(e){return encodeURIComponent(e)}}})();return t})()},257:function(e,t,n){"use strict";var r=this&&this.__awaiter||function(e,t,n,r){return new(n||(n=Promise))((function(a,i){function o(e){try{s(r.next(e))}catch(e){i(e)}}function l(e){try{s(r.throw(e))}catch(e){i(e)}}function s(e){var t;e.done?a(e.value):(t=e.value,t instanceof n?t:new n((function(e){e(t)}))).then(o,l)}s((r=r.apply(e,t||[])).next())}))},a=this&&this.__generator||function(e,t){var n,r,a,i,o={label:0,sent:function(){if(1&a[0])throw a[1];return a[1]},trys:[],ops:[]};return i={next:l(0),throw:l(1),return:l(2)},"function"==typeof Symbol&&(i[Symbol.iterator]=function(){return this}),i;function l(l){return function(s){return function(l){if(n)throw new TypeError("Generator is already executing.");for(;i&&(i=0,l[0]&&(o=0)),o;)try{if(n=1,r&&(a=2&l[0]?r.return:l[0]?r.throw||((a=r.return)&&a.call(r),0):r.next)&&!(a=a.call(r,l[1])).done)return a;switch(r=0,a&&(l=[2&l[0],a.value]),l[0]){case 0:case 1:a=l;break;case 4:return o.label++,{value:l[1],done:!1};case 5:o.label++,r=l[1],l=[0];continue;case 7:l=o.ops.pop(),o.trys.pop();continue;default:if(!((a=(a=o.trys).length>0&&a[a.length-1])||6!==l[0]&&2!==l[0])){o=0;continue}if(3===l[0]&&(!a||l[1]>a[0]&&l[1]<a[3])){o.label=l[1];break}if(6===l[0]&&o.label<a[1]){o.label=a[1],a=l;break}if(a&&o.label<a[2]){o.label=a[2],o.ops.push(l);break}a[2]&&o.ops.pop(),o.trys.pop();continue}l=t.call(e,o)}catch(e){l=[6,e],r=0}finally{n=a=0}if(5&l[0])throw l[1];return{value:l[0]?l[1]:void 0,done:!0}}([l,s])}}};Object.defineProperty(t,"__esModule",{value:!0});var i,o=n(940),l=n(347),s=window.location.origin,d=!0,u=document.getElementById("btn-exe-add"),c=document.getElementById("btn-grd-add");document.getElementById("btn-continue"),e.exports={addScriptEventListeners:function(){f()},saveStep:function(e){g(e)},displaySpinner:function(){o.util.displaySpinner()},hideSpinner:function(){o.util.hideSpinner()}};var f=function(){var e=document.getElementsByClassName("lnk-exe-delete"),t=document.getElementsByClassName("lnk-grd-delete"),n=Array.from(document.getElementsByTagName("input"));Array.from(document.getElementsByTagName("textarea")).forEach((function(e){e.addEventListener("keyup",(function(e){var t=e.target.nextElementSibling;t&&t.remove()}))})),n.forEach((function(e){e.addEventListener("keyup",(function(e){e.target.nextElementSibling.innerHTML=""}))})),u.addEventListener("click",m),c.addEventListener("click",p);for(var r=0;r<e.length;r++)e[r].onclick=function(e){var t,n;e.preventDefault();var r=null===(n=null===(t=e.target.parentElement)||void 0===t?void 0:t.parentElement)||void 0===n?void 0:n.parentElement;r&&r.remove()};for(r=0;r<t.length;r++)t[r].onclick=function(e){var t,n;e.preventDefault();var r=null===(n=null===(t=e.target.parentElement)||void 0===t?void 0:t.parentElement)||void 0===n?void 0:n.parentElement;r&&r.remove()}};function m(){var e=document.getElementById("add-exe-detail-div"),t=Array.from(document.getElementsByClassName("owner-exe"));return d&&h("SimpleWillOthers_GeneralWishes","General Wishes field cannot be blank"),d?(t.forEach((function(e,t){d&&v(e,"exe-firstname","Executor's First Name field cannot be blank"),d&&v(e,"exe-lastname","Executor's Last Name field cannot be blank")})),d?(e.insertAdjacentHTML("beforeend",'\n        <div class="d-flex col-8 owner-exe">\n\n                        <div class="col-6 pe-4 form-group">\n\n                            <input class="form-control mb-2 exe-firstname" type="text" name="exe-firstname" placeholder="First Name" data-val="true" data-val-required="The First Name of Executor field is required." id="SimpleWillExecutors_0__FirstNameOfExecutor" value="">\n\n                            <span class="text-danger field-validation-valid" data-valmsg-for="SimpleWillExecutors[0].FirstNameOfExecutor" data-valmsg-replace="true"></span>\n                        </div>\n\n                        <div class="col-6 form-group">\n\n                            <input class="form-control mb-2 exe-lastname" type="text" name="exe-lastname" placeholder="Last Name" data-val="true" data-val-required="The Last Name of Executor field is required." id="SimpleWillExecutors_0__LastNameOfExecutor" value="">\n\n                            <span class="text-danger field-validation-valid" data-valmsg-for="SimpleWillExecutors[0].LastNameOfExecutor" data-valmsg-replace="true"></span>\n                        </div>\n\n                        <a href="#" class="lnk-exe-delete bg-transparent trustee-btn py-2 p-0 border-0 col-auto form-group position-absolute d-flex align-items-center" style="right: 50px;">\n                        <span class="text-danger rds-pointer fs-6"><i class="bi bi-trash"></i></span>\n                    </a>\n                    </div>\n            '),void f()):(d=!0,i.focus(),!1)):(d=!0,i.focus(),!1)}function p(){var e=document.getElementById("add-grd-detail-div"),t=Array.from(document.getElementsByClassName("owner-grd"));return d&&h("SimpleWillOthers_GeneralWishes","General Wishes field cannot be blank"),d?(t.forEach((function(e,t){d&&v(e,"grd-firstname","Guardian's First Name field cannot be blank"),d&&v(e,"grd-lastname","Guardian's Last Name field cannot be blank")})),d?(e.insertAdjacentHTML("beforeend",'\n        <div class="d-flex col-8 owner-grd">\n\n                        <div class="col-6 pe-4 form-group">\n\n                            <input class="form-control mb-2 grd-firstname" type="text" name="grd-firstname" placeholder="First Name" data-val="true" data-val-required="The First Name of Guardian field is required." id="SimpleWillGuardians_0__FirstNameOfGuardian" value="">\n\n                            <span class="text-danger field-validation-valid" data-valmsg-for="SimpleWillGuardians[0].FirstNameOfGuardian" data-valmsg-replace="true"></span>\n                        </div>\n\n                        <div class="col-6 form-group">\n\n                            <input class="form-control mb-2 grd-lastname" type="text" name="grd-lastname" placeholder="Last Name" data-val="true" data-val-required="The Last Name of Guardian field is required." id="SimpleWillGuardians_0__LastNameOfGuardian" value="">\n\n                            <span class="text-danger field-validation-valid" data-valmsg-for="SimpleWillGuardians[0].LastNameOfGuardian" data-valmsg-replace="true"></span>\n                        </div>\n\n                        <a href="#" class="lnk-grd-delete bg-transparent trustee-btn py-2 p-0 border-0 col-auto form-group position-absolute d-flex align-items-center" style="right: 50px;">\n                                <span class="text-danger rds-pointer fs-6"><i class="bi bi-trash"></i></span>\n                            </a>\n                    </div>\n\n                    \n                    '),void f()):(d=!0,i.focus(),!1)):(d=!0,i.focus(),!1)}var g=function(e){return r(void 0,void 0,void 0,(function(){var t,n,r,u,c,f,m,p,g,y,b;return a(this,(function(a){switch(a.label){case 0:return e?(o.util.displaySpinner(),t=d?h("SimpleWillOthers_GeneralWishes","General Wishes field cannot be blank"):"",n=Array.from(document.getElementsByClassName("owner-exe")),r=Array.from(document.getElementsByClassName("owner-grd")),u={id:e,ownerId:"",ownerEmail:"",template:"",language:"",product:"",price:"",packageStatus:"",createDate:new Date},c={packageId:e,generalWishes:t},d?(f=n.map((function(t,n){var r=d?v(t,"exe-firstname","Executor's First Name field cannot be blank"):"",a=d?v(t,"exe-lastname","Executor's Last Name field cannot be blank"):"";return{packageId:e,ownerId:"",ownerEmail:"",packageType:"",firstNameOfExecutor:r,lastNameOfExecutor:a}})),d?(m=r.map((function(t,n){var r=d?v(t,"grd-firstname","Guardian's First Name field cannot be blank"):"",a=d?v(t,"grd-lastname","Guardian's Last Name field cannot be blank"):"";return{packageId:e,firstNameOfGuardian:r,lastNameOfGuardian:a}})),d?(p={trusteePackage:u,simpleWillExecutors:f,simpleWillGuardians:m,simpleWillOthers:c},g="".concat(s,"/Home/SaveSimpleWillOthers"),[4,l.dataService.postData(g,p)]):(d=!0,o.util.hideSpinner(),i.focus(),[2,!1])):(d=!0,o.util.hideSpinner(),i.focus(),[2,!1])):(d=!0,o.util.hideSpinner(),i.focus(),[2,!1])):[2,!1];case 1:return 201==(y=a.sent()).httpStatusCode?(window.location.href="".concat(s,"/Home/Summary?PackageId=").concat(e),[2]):(500==y.httpStatusCode?(b=y.data,swal(b.ExceptionType,b.ErrorDescription,"error")):(b=y.data,swal(b.ExceptionType,"Error connecting to backend server","error")),o.util.hideSpinner(),[2])}}))}))};function v(e,t,n){var r=e.getElementsByClassName(t)[0],a=r.nextElementSibling,o=r.value.trim();return null==o||null==o||0==o.trim().length?(a.innerHTML=n,i=r,d=!1):(d=!0,a.innerHTML=""),o}function h(e,t){var n=document.getElementById(e),r=n.nextElementSibling,a=n.value.trim();return null==a||null==a||0==a.trim().length?(r||(r=document.createElement("span")).classList.add("text-danger","ps-3","pt-2"),r.innerHTML=t,n.parentElement.appendChild(r),i=n,d=!1):(d=!0,r&&r.remove()),a}}},t={},n=function n(r){var a=t[r];if(void 0!==a)return a.exports;var i=t[r]={exports:{}};return e[r].call(i.exports,i,i.exports,n),i.exports}(257);trusteeSimplewillOthersScript=n})();