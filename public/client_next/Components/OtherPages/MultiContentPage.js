import { Paper } from "@mui/material";
import React from "react";
import { Card } from "react-bootstrap";
import OtherPagesContentCard from "./OtherPagesContentCard";

export default function MultiContentPage({
  contents,
  getLocal,
  getLocalByVal,
  selectedContent,
  setSelectedContent,
  loadSelectedContent,
  loadingContent,
  loadingContentsList,
}) {
  const [currentSelectedId, setCurrentSelectedId] = React.useState(null);
  const contentSelected = (content) => {
    setCurrentSelectedId(content.id);
    selectedContent && selectedContent.id == content.id
      ? setSelectedContent(null)
      : loadSelectedContent(content.id);
  };

  const ContentCard = ({ content }) => {
    if (loadingContent && currentSelectedId == content.id)
      return <ShowLoading />;

    if (selectedContent && selectedContent.id === content.id)
      return (
        <Paper style={{ padding: "6px" }}>
            <OtherPagesContentCard
              content={selectedContent}
              getLocal={getLocal}
              getLocalByVal={getLocalByVal}
            />
            <a
              style={{ margin: "0px 0px 0px 0px" }}
              className="btn btn-sm btn-warning"
              onClick={() => setSelectedContent(null)}
            >
              Close
            </a>
            <br />
          </Paper>
      );

    return null;
  };

  const ContentTitle = ({ content }) => {
    return (
      <a className="text-primary" onClick={() => contentSelected(content)}>
        <strong>{content.title}</strong>
      </a>
    );
  };

  const ShowLoading = () => {
    return (
      <strong>
        <br />
        {getLocal("Loading")}...
      </strong>
    );
  };

  return contents && contents.length > 0 ? (
    <Paper style={{ padding: "6px" }}>
      {loadingContentsList ? (
        <ShowLoading />
      ) : (
        <Card>
          <Card.Body>
            <ol>
              {contents.map((content, index) => (
                <li key={index}>
                  <ContentTitle content={content} />
                  <ContentCard content={content} />
                </li>
              ))}
            </ol>
          </Card.Body>
        </Card>
      )}
    </Paper>
  ) : null;
}
