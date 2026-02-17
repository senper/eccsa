import React from "react";
import { Row } from "react-bootstrap";
import { getServiceType } from "../api/servicetype";
import { useForm } from "react-hook-form";
import {
  saveRequest,
  getServiceRequest,
  excapeCharsReplacer,
} from "../../api/serviceRequest";
import {
  is_validate_email,
  is_validate_mobile,
  is_valid_Decimal,
  is_valid_Integer,
  is_valid_TIN,
  getNameFromId,
} from "../../utils/inputValidations";
import { getLegalProfileLookup } from "../../api/legalProfile";
import { getExportAgreementLookup } from "../../api/exportAgreement";
import ServiceRequestFormAttachments from "../../Components/ServiceRequest/ServiceRequestFormAttachments";
import TabularRequirementForm from "../../Components/ServiceRequest/TabularRequirementForm";
import { useRouter } from "next/router";
import { useSelector } from "react-redux";
import { getLocalByKey } from "../api/menu";
import { Grid, Snackbar } from "@mui/material";
import { AddAlert } from "@mui/icons-material";
import Custom404 from "../404";
import Login from "../login";
import ServiceRequestFormOptionals from "../../Components/ServiceRequest/ServiceRequestFormOptionals";

const emptyGuid = "00000000-0000-0000-0000-000000000000";
export default function ServiceRequestForm({ translations }) {
  const [user, setUser] = React.useState(
    useSelector((state) => state.auth.user)
  );

  const location = useRouter();
  const { locale } = location;
  const getLocal = (key) => {
    return getLocalByKey(key, translations, locale);
  };
  const getLocalByVal = (eng, local) => {
    return locale == "en" || !local ? eng : local;
  };
  const [saving, setSaving] = React.useState(false);
  //
  const [tabularResponses, setTabularResponses] = React.useState([]);
  const [attachments, setAttachments] = React.useState([]);
  //
  const [legalProfiles, setLegalProfiles] = React.useState([]);
  React.useEffect(() => {
    if (user) {
      getLegalProfileLookup(1).then((data) => {
        setLegalProfiles(data.data);
      });
    }
  }, [user]);
  const [exportAgreements, setExportAgreements] = React.useState([]);
  React.useEffect(() => {
    if (user) {
      getExportAgreementLookup(1).then((data) => {
        setExportAgreements(data.data);
      });
    }
  }, [user]);
  //
  const {
    register,
    handleSubmit,
    setValue,
    formState, //: { errors },
  } = useForm();
  //
  let serviceTypeIdValue = 0;
  let serviceRequestIdValue = null;
  if (location.query) {
    const { serviceTypeId, serviceRequestId } = location.query;
    serviceTypeIdValue = serviceTypeId;
    serviceRequestIdValue = serviceRequestId;
  }
  const [serviceType, setServiceType] = React.useState([]);
  React.useEffect(() => {
    if (user && serviceTypeIdValue) {
      getServiceType(serviceTypeIdValue).then((data) => {
        setServiceType(data.data);
      });
    }
  }, [user, serviceTypeIdValue]);
  //
  const [serviceRequest, setServiceRequest] = React.useState([]);
  const [serviceRequestDocs, setServiceRequestDocs] = React.useState([]);
  const [optionalEntries, setOptionalEntries] = React.useState([]);
  React.useEffect(() => {
    if (user && serviceRequestIdValue) {
      getServiceRequest(serviceRequestIdValue).then((data) => {
        setServiceRequest(data.data);
        setServiceRequestDocs(data.data.responses.$values);
        //
        const responses = data.data.responses.$values.filter(
          (r) => r.requirement.inputMethodValue == 11
        );
        const resultList = [];
        responses.forEach((r) => {
          if (r.tabularResponse && r.tabularResponse.$values) {
            let rowIds = r.tabularResponse.$values.map((t) => t.rowId);
            //get distinct rows
            rowIds = rowIds.filter((value, index, self) => {
              return self.indexOf(value) === index;
            });
            rowIds.forEach((v) => {
              const rowData = r.tabularResponse.$values.filter(
                (t) => t.rowId == v
              );
              let inputDataStr = [];
              rowData.forEach((d) =>
                inputDataStr.push(
                  `"${d.text}": "${excapeCharsReplacer(d.value)}"`
                )
              );
              resultList.push({
                id: v,
                reqId: r.requirement.id,
                row: JSON.parse(`{${inputDataStr.toString()}}`),
              });
            });
          }
        });
        setTabularResponses(resultList);
        //
        if (
          data.data &&
          data.data.optionalEntries &&
          data.data.optionalEntries.$values
        ) {
          setOptionalEntries(data.data.optionalEntries.$values);
        }
        //
      });
    }
  }, [user, serviceRequestIdValue]);
  //
  function isReqReadOnly(req) {
    const responseDoc = serviceRequestDocs.find(
      (r) => r.requirementId === req.id
    );
    return !(
      !serviceRequestIdValue ||
      serviceRequest.requestStatusValue === 0 ||
      (responseDoc && responseDoc.reviewStatusValue === 3)
    );
  }
  //
  function getInput(r) {
    const idStr = r.id.toString();
    const name = idStr.substr(idStr.length - 15).replace("-", "");
    const responseDoc = serviceRequestDocs.find(
      (r) => r.requirementId === idStr
    );
    const readOnly =
      !serviceRequestIdValue ||
      serviceRequest.requestStatusValue === 0 ||
      (responseDoc && responseDoc.reviewStatusValue === 3) ||
      (serviceRequest.isOpponent && responseDoc.reviewStatusValue === 0)
        ? ""
        : "readOnly";
    const disabled = readOnly === "" ? "" : "disabled";
    if (r.inputMethodValue === 1) {
      let docLink = <></>;
      if (responseDoc) {
        const fileName = responseDoc.id
          .substr(responseDoc.id.length - 15)
          .replace("-", "");
        const relativePath = `${serviceType.serviceOwnerStr.toLowerCase()}/documents/${fileName}${
          responseDoc.textResponse
        }`;
        docLink = (
          <a
            href={`${process.env.REACT_APP_API_URL}/${relativePath}`}
            target="_blank"
            rel="noreferrer"
          >
            {getLocal("Saved Document")}
          </a>
        );
      }
      return (
        <>
          {responseDoc && responseDoc.textResponse ? docLink : null}
          {readOnly === "" ? (
            <input
              title={getLocal(r.descriptionEng, r.descriptionLocal)}
              type="file"
              name={name}
              className="form-control "
              {...register(name)}
              style={{ borderColor: formState.errors[name] ? "red" : "" }}
            />
          ) : null}
        </>
      );
    } else if (r.inputMethodValue === 2) {
      return (
        <input
          {...register(name, { validate: (value) => is_valid_Integer(value) })}
          type="number"
          name={name}
          max="2000000000"
          {...{ readOnly }}
          className="form-control "
          title={
            formState.errors[name]
              ? getLocal("Invalid Number Value")
              : getLocal(r.descriptionEng, r.descriptionLocal)
          }
          style={{ borderColor: formState.errors[name] ? "red" : "" }}
        />
      );
    } else if (r.inputMethodValue === 3) {
      return (
        <input
          {...register(name, { validate: (value) => is_valid_Decimal(value) })}
          type="number"
          name={name}
          step={"any"}
          {...{ readOnly }}
          className="form-control "
          title={
            formState.errors[name]
              ? getLocal("Invalid Decimal Number")
              : getLocal(r.descriptionEng, r.descriptionLocal)
          }
          style={{ borderColor: formState.errors[name] ? "red" : "" }}
        />
      );
    } else if (r.inputMethodValue === 4) {
      return (
        <input
          {...register(name, { required: true })}
          type="date"
          name={name}
          title={getLocal(r.descriptionEng, r.descriptionLocal)}
          {...{ readOnly }}
          maxLength="100"
          className="form-control "
          style={{ borderColor: formState.errors[name] ? "red" : "" }}
        />
      );
    } else if (r.inputMethodValue === 9) {
      //Invoice and Payment
      /* return (
        <input
          {...register(name)}
          type="text"
          name={name}
          {...{ readOnly }}
          maxLength="100"
          className="form-control "
          title={formState.errors[name] ? "Invalid Input" : ""}
          style={{ borderColor: formState.errors[name] ? "red" : "" }}
        />
      ); */
    } else if (r.inputMethodValue === 5) {
      return (
        <input
          {...register(name, { required: true })}
          type="text"
          name={name}
          maxLength={50}
          title={getLocal(r.descriptionEng, r.descriptionLocal) + '(less than 50 characters)'}
          {...{ readOnly }}
          className="form-control "
          style={{ borderColor: formState.errors[name] ? "red" : "" }}
        />
      );
    } else if (r.inputMethodValue === 6) {
      return (
        <textarea
          {...register(name, { required: true })}
          rows="4"
          name={name}
          title={getLocal(r.descriptionEng, r.descriptionLocal)}
          {...{ readOnly }}
          className="form-control "
          style={{ borderColor: formState.errors[name] ? "red" : "" }}
        ></textarea>
      );
    } else if (r.inputMethodValue === 15) {
      return (
        <textarea
          {...register(name, { required: true })}
          rows="4"
          name={name}
          maxLength={200}
          title={getLocal(r.descriptionEng, r.descriptionLocal) + '(less than 200 characters)'}
          {...{ readOnly }}
          className="form-control "
          style={{ borderColor: formState.errors[name] ? "red" : "" }}
        ></textarea>
      );
    } else if (r.inputMethodValue === 7) {
      //LegalDocument
      return (
        <>
          <select
            {...register(name, { required: true })}
            name={name}
            {...{ disabled }}
            className="form-control "
            title={
              formState.errors[name]
                ? getLocal("Invalid Input")
                : getLocal(r.descriptionEng, r.descriptionLocal)
            }
            style={{ borderColor: formState.errors[name] ? "red" : "" }}
          >
            {legalProfiles.$values
              ? legalProfiles.$values.map((lp, index) => (
                  <option key={index} value={lp.id}>
                    {lp.name}
                  </option>
                ))
              : null}
          </select>
        </>
      );
    } else if (r.inputMethodValue === 8) {
      //TradeAgreement
      return (
        <>
          <select
            {...register(name, { required: true })}
            name={name}
            {...{ disabled }}
            className="form-control "
            title={
              formState.errors[name]
                ? getLocal("Invalid Input")
                : getLocal(r.descriptionEng, r.descriptionLocal)
            }
            style={{ borderColor: formState.errors[name] ? "red" : "" }}
          >
            {exportAgreements.$values
              ? exportAgreements.$values.map((lp, index) => (
                  <option key={index} value={lp.id}>
                    {lp.name}
                  </option>
                ))
              : null}
          </select>
        </>
      );
    } else if (r.inputMethodValue === 10) {
      //Lookup
      return (
        <>
          <select
            {...register(name, { required: true })}
            name={name}
            {...{ disabled }}
            className="form-control "
            title={
              formState.errors[name]
                ? getLocal("Invalid Input")
                : getLocal(r.descriptionEng, r.descriptionLocal)
            }
            style={{ borderColor: formState.errors[name] ? "red" : "" }}
          >
            {serviceType.requirementLookups.$values &&
            serviceType.requirementLookups.$values.find(
              (l) => l.id == r.lookupTypeId
            )
              ? serviceType.requirementLookups.$values
                  .find((l) => l.id == r.lookupTypeId)
                  .lookupValues.$values.map((lp, index) => (
                    <option key={index} value={lp.id}>
                      {lp.name}
                    </option>
                  ))
              : null}
          </select>
        </>
      );
    } else if (r.inputMethodValue === 12) {
      return (
        <input
          {...register(name, { required: true })}
          type="email"
          name={name}
          title={getLocal(r.descriptionEng, r.descriptionLocal)}
          {...{ readOnly }}
          maxLength="100"
          className="form-control "
          style={{ borderColor: formState.errors[name] ? "red" : "" }}
        />
      );
    } else if (r.inputMethodValue === 13 || r.inputMethodValue === 14) {
      return (
        <input
          {...register(name, {
            required: true,
            validate: (value) => is_validate_mobile(value),
          })}
          type="text"
          name={name}
          title={getLocal(r.descriptionEng, r.descriptionLocal)}
          {...{ readOnly }}
          maxLength="15"
          className="form-control "
          style={{ borderColor: formState.errors[name] ? "red" : "" }}
        />
      );
    }
  }
  function setInputValue(r) {
    if (formState && formState.isDirty) return;
    if (r.inputMethodValue !== 1 && r.id !== emptyGuid) {
      const idStr = r.id.toString();
      const name = idStr.substr(idStr.length - 15).replace("-", "");
      const responseDoc = serviceRequestDocs.find(
        (r) => r.requirementId === idStr
      );
      let value = responseDoc ? responseDoc.response : "";
      if (
        responseDoc &&
        (responseDoc.requirement.inputMethodValue === 7 ||
          responseDoc.requirement.inputMethodValue == 8 ||
          responseDoc.requirement.inputMethodValue == 10)
      ) {
        value = responseDoc.lookupValueId;
      }
      setValue(name, value);
    }
  }
  function Redirect() {
    if (serviceRequestIdValue) {
      location.push(
        `/myServices/serviceRequestDetailPage?serviceRequestId=${serviceRequestIdValue}&serviceTypeId=${serviceTypeIdValue}`
      );
    } else {
      location.push("/myServices?navTo=request");
    }
  }
  function onSubmit(inputData) {
    setSaving(true);
    var formData = new FormData();
    formData.append(
      "Id",
      serviceRequestIdValue ? serviceRequestIdValue : emptyGuid
    );
    formData.append("ServiceTypeId", serviceTypeIdValue);
    for (const [key, value] of Object.entries(inputData)) {
      const dataValue = value && value[0] && value[0].name ? value[0] : value;
      formData.append(key, dataValue);
    }
    //
    optionalEntries.forEach((att) => {
      formData.append("OptionalEntryLabel", att.label);
      formData.append("OptionalEntryValue"+att.label, att.value);
    });
    //    
    attachments.forEach((att) => {
      formData.append("AttachmentTitle", att.title);
      formData.append(att.title, att.file);
    });
    //
    serviceType.serviceRequirements.$values
      .filter((r) => r.inputMethodValue == 11)
      .forEach((tr) => {
        const formattedData = [];
        tabularResponses
          .filter((t) => t.reqId == tr.id)
          .forEach((v) => {
            for (const [key, value] of Object.entries(v.row)) {
              formattedData.push({ rowId: v.id, text: key, value: value });
            }
          });
        formData.append(getNameFromId(tr.id), JSON.stringify(formattedData));
      });
    saveRequest(formData)
      .then((data) => {
        setSaving(false);
        if (data.data.saved) {
          Redirect();
        } else {
          if (data.data.erorMsg) {
            var splited = data.data.erorMsg.split("\r\n");
            var htmlError = (
              <div>
                {splited.map((val, i) => (
                  <span key={i}>
                    {val}
                    <br />
                  </span>
                ))}
              </div>
            );
            setErrorMsg(htmlError);
          }
          showNotification("tr", false);
        }
      })
      .catch((error) => {
        setSaving(false);
        // console.log(error);
        showNotification("tr", false);
      });
  }
  const [error, setError] = React.useState(false);
  const [errorMsg, setErrorMsg] = React.useState(null);
  const showNotification = (place, useTimeOut = true) => {
    switch (place) {
      case "tr":
        if (!error) {
          setError(true);
          if (useTimeOut) {
            setTimeout(function () {
              setError(false);
              setErrorMsg(null);
            }, 5000);
          }
        }
        break;
      default:
        break;
    }
  };
  function compare(a, b) {
    if (a.orderNo < b.orderNo) {
      return -1;
    }
    if (a.orderNo > b.orderNo) {
      return 1;
    }
    return 0;
  }
  const getInputLabel = (req) => {
    if (!req || req.inputMethodValue === 9 || req.requirementTypeValue == 2)
      return;
    var value = getLocalByVal(req.label, req.labelLocal);
    if (req.requirementType === "Header") {
      return (
        <label>
          <h3>{value}</h3>
        </label>
      );
    } else {
      return <label>{value}</label>;
    }
  };

  if (!user) {
    //return <Custom404 statusCode={404} />;
    const formUrl = `/myServices/serviceRequestForm?serviceTypeId=${serviceTypeIdValue}`;
    return <Login translations={translations} query={formUrl} />;
  }
  return (
    <>
      {/* <Topbar/> */}
      {serviceType ? (
        <div className="row">
          <div className="bg">
            {getLocalByVal(serviceType.nameEng, serviceType.nameLocal)}
          </div>
          <Grid container spacing={2}>
            <Grid item sm={1} md={2}></Grid>{" "}
            <Grid item sm={11} md={10}>
              <form
                onSubmit={handleSubmit(onSubmit)}
                encType="multipart/form-data"
              >
                {serviceType.serviceRequirements &&
                serviceType.serviceRequirements.$values ? (
                  <>
                    <Grid item sm={1} md={2}></Grid>{" "}
                    <Grid item sm={10} md={6}>
                      {serviceType.serviceRequirements.$values
                        .filter(
                          (r) =>
                            r.inputMethodValue != 11 &&
                            r.isForOpponent ==
                              (user != null &&
                                serviceRequestIdValue != null &&
                                serviceRequest.requestingUserId != user.id)
                        )
                        .sort(compare)
                        .map((req, index) => (
                          <Row key={index} xs={10} md={8}>
                            {getInputLabel(req)}
                            {getInput(req)} {setInputValue(req)}
                            {/* {errors[req.id]} */}
                          </Row>
                        ))}
                    </Grid>
                    <br />
                    <Grid item sm={11} md={10}>
                      {serviceType.serviceRequirements.$values
                        .filter((r) => r.inputMethodValue == 11)
                        .sort(compare)
                        .map((req, index) => (
                          <TabularRequirementForm
                            key={index}
                            requirement={req}
                            tabularSource={serviceType.tabularRequirements.$values.find(
                              (l) => l.id == req.lookupTypeId
                            )}
                            tabularResponses={tabularResponses.filter(
                              (s) => s.reqId == req.id
                            )}
                            setTabularResponses={setTabularResponses}
                            isReadOnly={isReqReadOnly(req)}
                            getLocal={getLocal}
                            getLocalByVal={getLocalByVal}
                            lookups={serviceType.requirementLookups.$values}
                          />
                        ))}
                    </Grid>
                  </>
                ) : null}
                {serviceType &&
                serviceType.allowOptionalEntries &&
                (!serviceRequestIdValue ||
                  serviceRequestIdValue == emptyGuid) &&
                serviceType.optionalEntryLabels &&
                serviceType.optionalEntryLabels.$values ? (
                  <>
                    <br />
                    <Grid item sm={1} md={2}></Grid>{" "}
                    <Grid item sm={12} md={10}>
                      <ServiceRequestFormOptionals
                        optionalEntries={optionalEntries}
                        setOptionalEntries={setOptionalEntries}
                        optionalEntryLookupValues={
                          serviceType.optionalEntryLabels.$values
                        }
                        showNotification={showNotification}
                        setErrorMsg={setErrorMsg}
                        getLocal={getLocal}
                        getLocalByVal={getLocalByVal}
                      />
                    </Grid>
                  </>
                ) : null}
                {serviceType &&
                serviceType.allowAdditionalAttachments &&
                (!serviceRequestIdValue ||
                  serviceRequestIdValue == emptyGuid) ? (
                  <>
                    <br />
                    <Grid item sm={1} md={2}></Grid>{" "}
                    <Grid item sm={12} md={10}>
                      <ServiceRequestFormAttachments
                        attachments={attachments}
                        setAttachments={setAttachments}
                        showNotification={showNotification}
                        setErrorMsg={setErrorMsg}
                        getLocal={getLocal}
                        getLocalByVal={getLocalByVal}
                      />
                    </Grid>
                  </>
                ) : null}
                <Grid item sm={1} md={2}></Grid>{" "}
                <Grid item sm={12} md={6}>
                  <button
                    type="submit"
                    className="btn btn-success"
                    style={{ marginTop: "0" }}
                    disabled={saving}
                  >
                    {saving ? getLocal("Saving") : getLocal("Save")}
                  </button>
                  <button
                    type="button"
                    className="btn btn-warning"
                    onClick={Redirect}
                    style={{ marginTop: "0" }}
                  >
                    {getLocal("Cancel")}
                  </button>
                </Grid>
              </form>
              <br />
            </Grid>
            <Grid item sm={1} md={2}></Grid>
          </Grid>
        </div>
      ) : null}
      <Grid container spacing={2}>
        <br />
        <br />

        <Snackbar
          place="tr"
          color="danger"
          icon={AddAlert}
          message={
            errorMsg || !error ? errorMsg : getLocal("Failed to Save request")
          }
          open={error}
          onClose={() => {
            setError(false);
            setErrorMsg(null);
          }}
        />
      </Grid>
    </>
  );
}

// export default App;
