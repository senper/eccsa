import React, { useState } from "react";
import { Grid, Snackbar } from "@material-ui/core";
import { Form, Row } from "react-bootstrap";
import "bootstrap/dist/css/bootstrap.min.css";
import { AddAlert } from "@material-ui/icons";
import "../../App.css";
import "bootstrap/dist/css/bootstrap.css";
import Footer from "../../component/Footer/Footer";
import { useLocation } from "react-router-dom";
import { useForm } from "react-hook-form";
import { saveResponse, getUserSurvey } from "../../services/survey";
import { useHistory } from "react-router-dom";
import Select from "react-select";

const emptyGuid = "00000000-0000-0000-0000-000000000000";
export default function SurveyQuestionForm() {
  const location = useLocation();
  const history = useHistory();
  const {
    register,
    handleSubmit,
    setValue,
    getValues ,
    formState, //: { errors },
  } = useForm();
  let surveyIdValue = 0;
  if (location.state) {
    const { surveyId } = location.state;
    surveyIdValue = surveyId;
  }
  const [survey, setSurvey] = React.useState([]);
  React.useEffect(() => {
    if (surveyIdValue) {
      getUserSurvey(surveyIdValue).then((data) => {
        setSurvey(data.data);
      });
    }
  }, [surveyIdValue]);
  /* const handleSelectChange = (name, selected) => {
    let value = "";
    if (selected) {
      selected.map((v) => (value += v.value));
      setValue(name, value);
    } else {
      setValue(name, "");
    }
  }; */
  function getInput(q) {
    const idStr = q.id.toString();
    const name = idStr.substr(idStr.length - 15).replace("-", "");
    if (q.questionTypeValue === 2) {
      return (
        <div /* onChange={(e) => { setValueChange([e.target.id, e.target.checked]); }}> */>
          {q.simpleChoices
            ? q.simpleChoices.$values.sort(compare).map((choice) => (
                <div>
                  <input
                    {...register(name)}
                    type="checkbox"
                    value={choice.id}
                    name={name}
                  />{" "}
                  {choice.orderNo.trim()}. {choice.choiceText.trim()}
                </div>
              ))
            : null}
        </div>
      );
    } else if (q.questionTypeValue === 1) {
      return (
        <div /* onChange={(e) => { setValueChange([e.target.id, e.target.checked]); }} */>
          {q.simpleChoices
            ? q.simpleChoices.$values.sort(compare).map((choice) => (
                <div>
                  <input
                    {...register(name)}
                    type="radio"
                    value={choice.id}
                    name={name}
                  />{" "}
                  {choice.orderNo.trim()}. {choice.choiceText.trim()}
                </div>
              ))
            : null}
        </div>
      );
    } else if (q.questionTypeValue === 3) {
      return (
        <input
          {...register(name)}
          type="number"
          name={name}
          class="form-control "
          min={q.minRange}
          max={q.maxRange}
        />
      );
    } else if (q.questionTypeValue === 4) {
      return (
        <input
          {...register(name)}
          type="text"
          name={name}
          class="form-control "
        />
      );
    }
  }
  function setInputValue(q) {
    if (formState.isDirty || !q.response) return;
    if (q.id !== emptyGuid) {
      const idStr = q.id.toString();
      const name = idStr.substr(idStr.length - 15).replace("-", "");
      const response = q.response;
      let value = null;
      if (q.questionTypeValue === 2) {
        if (response.choices && q.simpleChoices) {
          const options = [];
          q.simpleChoices.$values.map((choice) => {
            const hasValue = response.choices.$values.filter(
              (sValue) => sValue.choiceId == choice.id
            );
            const added = options.filter((value) => value.value == choice.id);
            if (hasValue.length > 0 && added.length <= 0) {
              //options.push({ value: choice.id, label: choice.choiceText });
              options.push(choice.id);
            }
          });
          value = options;
        }
      } else if (q.questionTypeValue === 1) {
        if (response.choices && response.choices.$values.length > 0) {
          value = response.choices.$values[0].choiceId;
        }
      } else if (q.questionTypeValue === 3) {
        value = response.numericResponse;
      } else if (q.questionTypeValue === 4) {
        value = response.textualResponse;
      }
      setValue(name, value);
    }
  }
  function onSubmit(inputData) {
    var formData = new FormData();
    formData.append("Id", surveyIdValue ? surveyIdValue : emptyGuid);
    for (const [key, value] of Object.entries(inputData)) {
      const dataValue = value && value[0] && value[0].name ? value[0] : value;
      formData.append(key, dataValue);
    }
    saveResponse(surveyIdValue, formData)
      .then((data) => {
        if (data.data.saved) {
          setErrorMsg("Saved!");
        } else if (data.data.errorMsg) {
          setErrorMsg(data.data.errorMsg);
        }
        showNotification("tr");
      })
      .catch((error) => {
        showNotification("tr");
      });
  }
  const [error, setError] = React.useState(false);
  const [errorMsg, setErrorMsg] = React.useState(null);
  const showNotification = (place) => {
    switch (place) {
      case "tr":
        if (!error) {
          setError(true);
          setTimeout(function () {
            setError(false);
            setErrorMsg(null);
          }, 6000);
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
  function Redirect() {
    history.push({
      pathname: "/myServices",
      state: {
        navTo: "survey",
      },
    });
  }
 /*   const [valuechange, setValueChange] = React.useState(null);
  function canShow(q){
    if(!q.parentChoiceId) return true;
    const parentChoice = survey.choices.$values.find((v) => v.id == q.parentChoiceId);  
    if(!parentChoice) return true;
    const idStr = parentChoice.questionId.toString();
    const name = idStr.substr(idStr.length - 15).replace("-", "");
    const parentQvalue = getValues(name);
    //
    return parentQvalue == q.parentChoiceId || (parentQvalue != null && parentQvalue.includes && parentQvalue.includes(q.parentChoiceId));
  } */
  return (
    <div>
      {/* <Topbar/> */}
      {survey ? (
        <div class="row">
          <div class="bg">{survey.selectedSurvey ? survey.selectedSurvey.title : survey.title}</div>
          <Grid item xs={1} sm={2}></Grid>{" "}
          <Grid item xs={10} sm={6}>
            <Form onSubmit={handleSubmit(onSubmit)}>
              {survey.questions
                ? survey.questions.$values.sort(compare).map((question) => (
                  // canShow(question) ?
                    <Row xs={10} md={8}>
                      <label>
                        {question.orderNo.trim()}. {question.question.trim()} <small><strong>{question.parentText}</strong></small>
                      </label>
                      {getInput(question)} {setInputValue(question)}
                      {/* {errors[req.id]} */}
                    </Row> //: null
                  ))
                : null}
              <br />
              <button type="submit" class="btn btn-success">
                Save
              </button>
              <button type="button" class="btn btn-warning" onClick={Redirect}>
                Cancel
              </button>
            </Form>
            <br />
          </Grid>
          <Grid item xs={1} sm={2}></Grid>
        </div>
      ) : null}
      <Grid container spacing={2}>
        <br />
        <br />
  
        <Snackbar
          place="tr"
          color="danger"
          icon={AddAlert}
          message={errorMsg ? errorMsg : "Failed to Save response"}
          open={error}
          closeNotification={() => setError(false)}
          close
        />
      </Grid>
    </div>
  );
}

// export default App;
