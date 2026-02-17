import React from "react";
import { Card } from "react-bootstrap";

export default function OtherPagesContentCard({
  content,
  getLocal,
  getLocalByVal,
}) {
  const formateProfile = () => {
    let profile = getLocalByVal(content.content, content.contentLocal);
    profile = profile.replaceAll(
      'src="/pr/text_media',
      `src="${process.env.NEXT_PUBLIC_API_URL}/pr/text_media`
    );
    return profile;
  };
  return (
    <Card>
      <Card.Body style={{ display: "block", overflowX: "auto" }}>
        <div 
          dangerouslySetInnerHTML={{ __html: formateProfile() }}
        />
      </Card.Body>
    </Card>
  );
}
